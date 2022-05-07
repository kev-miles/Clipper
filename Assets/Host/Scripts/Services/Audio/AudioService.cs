using System;
using System.Collections.Generic;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using UniRx;

namespace Host.Scripts.Services.Audio
{
    public class AudioService : IAudioService
    {
        public float MusicVolume => _musicVolume;
        public float SoundVolume => _soundVolume;
        
        private readonly IEventService _eventService;
        private readonly ISettingsService _settingsService;

        private float _musicVolume;
        private float _soundVolume;

        public AudioService(IEventService eventService, ISettingsService settingsService)
        {
            _eventService = eventService;
            _settingsService = settingsService;
        }

        public IObservable<Unit> Initialize()
        {
            return Observable.Create<Unit>(emitter =>
            {
                SetInitialVolumes();
                emitter.OnNext(Unit.Default);
                emitter.OnCompleted();
                return Disposable.Empty;
            });
        }
        
        public Dictionary<string, float> GetAudioVolumes() => new Dictionary<string, float>()
        {
            {SettingName.MUSIC_VOLUME, _musicVolume},
            {SettingName.SFX_VOLUME, _soundVolume}
        };

        public void UpdateMusicVolume(float volume)
        {
            _musicVolume = volume;
            SendAudioSettingsChangedEvent();
        }

        public void UpdateSoundVolume(float volume)
        {
            _soundVolume = volume;
            SendAudioSettingsChangedEvent();
        }

        private void SetInitialVolumes()
        {
            _musicVolume = int.Parse(_settingsService.GetSetting(SettingName.MUSIC_VOLUME)) / 10f;
            UpdateMusicVolume(_musicVolume);
            _soundVolume = int.Parse(_settingsService.GetSetting(SettingName.SFX_VOLUME)) / 10f;
            UpdateSoundVolume(_soundVolume);
        }

        private void SendAudioSettingsChangedEvent()
        {
            _eventService.SendEvent(new AudioSettingsChangedEvent().WithParameters(BuildEventParameters()));
        }
        
        private Dictionary<string, float> BuildEventParameters()
        {
            return new Dictionary<string, float>()
            {
                {SettingName.MUSIC_VOLUME, _musicVolume},
                {SettingName.SFX_VOLUME, _soundVolume}
            };
        }
    }
}
