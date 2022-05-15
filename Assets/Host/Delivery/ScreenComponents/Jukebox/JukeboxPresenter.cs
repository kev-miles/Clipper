using System;
using System.Collections.Generic;
using System.Linq;
using Host.Delivery.ScreenComponents.Jukebox.Infrastructure;
using Host.Delivery.Screens.PresenterProvider;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using ScreenComponents.Jukebox;
using UniRx;
using UnityEngine;

namespace Host.Delivery.ScreenComponents.Jukebox
{
    public class JukeboxPresenter : IPresenter
    {
        private AudioClip _introAudioClip;
        private Song[] _gameplaySongs;
        private IJukeboxView _view;
        
        private IEventService _eventService;
        private ITranslationService _translator;
        private IDisposable _songDisposable;
        private int _songIndex = -1;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private readonly string _nowPlayingTid = "tid_now_playing";
        
        public JukeboxPresenter(IEventService eventService, ITranslationService translator, IJukeboxView view, 
            Dictionary<string,float> initialSettings)
        {
            _view = view;
            _eventService = eventService;
            _translator = translator;
            _introAudioClip = Resources.LoadAll<AudioClip>("Music/Intro").FirstOrDefault();
            _gameplaySongs = BuildSongList(Resources.LoadAll<AudioClip>($"Music/Gameplay"));
            _view.OnStart += LocalizeNowPlaying;
            _view.OnUnload += Unload;
            _view.UpdateAudioSourceSettings(initialSettings[SettingName.MUSIC_VOLUME]);
            SubscribeToApplicationEvents();
        }
        
        public void Present()
        {
            PlayIntroSong();
        }
        
        private Song[] BuildSongList(AudioClip[] audioClips)
        {
            var songList = new List<Song>();
            foreach (var clip in audioClips)
            {
                var song = new Song();
                var songTitle = GetSongTitle(clip.name);
                song.name = songTitle.FirstOrDefault();
                song.artist = songTitle.LastOrDefault();
                song.clip = clip;
                song.length = clip.length;
                songList.Add(song);
            }
            return songList.ToArray();
        }

        private string[] GetSongTitle(string audioClipName)
        {
            return audioClipName.Split('-');
        }
        
        private void PlayIntroSong()
        {
            _view.PlayIntroSong(_introAudioClip);
        }
        
        private void PlayNextSong()
        {
            _songIndex = (_songIndex + 1) % _gameplaySongs.Length;
            var song = _gameplaySongs[_songIndex];
            TrackSongPlayingTime(song);
            _view.PlaySong(song);
        }

        private void TrackSongPlayingTime(Song song)
        {
            _songDisposable?.Dispose();
            _songDisposable = Observable.Timer(TimeSpan.FromSeconds(song.length)).Subscribe(_ => PlayNextSong());
        }

        private void SubscribeToApplicationEvents()
        {
            _eventService.On<PressStartEvent>().Subscribe(_ => PlayNextSong()).AddTo(_disposable);
            _eventService.On<AudioSettingsChangedEvent>().Subscribe(e 
                => UpdateAudioSourceSettings(e.Parameters)).AddTo(_disposable);
            _eventService.On<LanguageChangedEvent>().Subscribe(_ 
                => LocalizeNowPlaying()).AddTo(_disposable);
        }
        
        private void LocalizeNowPlaying()
        {
            _view.UpdateNowPlayingText(_translator.Translate(_nowPlayingTid));
        }

        private void UpdateAudioSourceSettings(Dictionary<string,float> eventParameters)
        {
            _view.UpdateAudioSourceSettings(eventParameters[SettingName.MUSIC_VOLUME]);
        }

        private void Unload()
        {
            _songDisposable?.Dispose();
            _disposable?.Dispose();
        }
    }
}