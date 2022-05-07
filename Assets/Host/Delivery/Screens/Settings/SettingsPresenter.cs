using System;
using System.Collections.Generic;
using Host.Delivery.Screens.PresenterProvider;
using Host.Delivery.Screens.Settings.Infrastructure;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using UniRx;

namespace Host.Delivery.Screens.Settings
{
    public class SettingsPresenter : IPresenter
    {
        private readonly ISettingsView _view;
        private readonly IEventService _eventService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ITranslationService _translationService;
        private readonly IAudioService _audioService;
        
        private Dictionary<string,string> _settings;
        private Dictionary<string, Action<object>> _methodDictionary;
        private readonly INavigationService _navigationService;

        public SettingsPresenter(IEventService eventService, ISettingsService settingsService, 
            INavigationService navigationService, ITranslationService translationService, ISaveLoadService saveLoadService,
            IAudioService audioService, ISettingsView view)
        {
            _eventService = eventService;
            _settings = settingsService.GetSettings();
            _translationService = translationService;
            _navigationService = navigationService;
            _saveLoadService = saveLoadService;
            _audioService = audioService;
            _view = view;
            BuildMethodDictionary();
            SubscribeToViewEvents();
            SubscribeToApplicationEvents();
        }

        public void Present()
        {
            _view.Show();
            _navigationService.OpenView(_view);
        }

        private void BuildMethodDictionary()
        {
            _methodDictionary = new Dictionary<string, Action<object>>
            {
                {SettingName.LANGUAGE, OnLanguageUpdated},
                {SettingName.MUSIC_VOLUME, OnMusicVolumeUpdated},
                {SettingName.SFX_VOLUME, OnSoundVolumeUpdated},
                {SettingName.SAVE_SLOT, OnSaveSlotUpdated}
            };
        }
        
        private void OnLanguageUpdated(object value)
        {
            var languageKey = _translationService.GetLanguageKeyForIndex(Convert.ToInt32(value));
            _settings[SettingName.LANGUAGE] = languageKey;
            _translationService.UpdateLanguage(languageKey);
        }

        private void OnMusicVolumeUpdated(object volume)
        {
            var volumeAsInt = (int)((float)volume * 10);
            _settings[SettingName.MUSIC_VOLUME] = volumeAsInt.ToString();
            _audioService.UpdateMusicVolume((float)volume);
        }

        private void OnSoundVolumeUpdated(object volume)
        {
            var volumeAsInt = (int)((float)volume * 10);
            _settings[SettingName.SFX_VOLUME] = volumeAsInt.ToString();
            _audioService.UpdateSoundVolume((float)volume);
        }

        private void OnSaveSlotUpdated(object slot)
        {
            var slotAsInt = Convert.ToInt32(slot);
            _settings[SettingName.SAVE_SLOT] = slotAsInt.ToString();
            _saveLoadService.UpdateActiveSaveSlot(slotAsInt);
        }

        private void SubscribeToViewEvents()
        {
            _view.OnLoad += TranslateTexts;
            _view.OnLoad += SetSelectorValues;
            _view.OnBackPressed += () =>
            {
                SaveSettings();
                _navigationService.CloseView();
            };
            _view.OnIntroFinished += () => {};
            _view.OnSettingUpdated += val => _methodDictionary[val.settingName](val.settingValue);
        }

        private void SetSelectorValues()
        {
            _view.SetSelectorValues(new Dictionary<string, string[]>()
            {
                {SettingName.LANGUAGE, 
                    new []{_translationService.Translate("tid_english"),_translationService.Translate("tid_spanish")}},
                {SettingName.SAVE_SLOT, 
                    new []{_translationService.Translate("tid_slot_1"),_translationService.Translate("tid_slot_2"),
                        _translationService.Translate("tid_slot_3")}}
            });
            SetSettingsValues();
        }

        private void SaveSettings()
        {
            _eventService.SendEvent(new SettingsUpdatedEvent().WithParameters(_settings));
        }

        private void SubscribeToApplicationEvents()
        {
            _eventService.On<LanguageChangedEvent>().Subscribe(_ => TranslateTexts());
        }
        
        private void TranslateTexts()
        {
            var viewTIDs = _view.GetTranslaitonIDs();
            _view.SetTexts(_translationService.TranslateAll(viewTIDs));
            SetSelectorValues();
        }
        
        private void SetSettingsValues()
        {
            _view.SetInitialSliders(_audioService.GetAudioVolumes());
            _view.SetInitialSelectors(GetFormattedSelectorValues());
        }

        private Dictionary<string, int> GetFormattedSelectorValues()
        {
            return new Dictionary<string, int>()
            {
                { SettingName.LANGUAGE, _translationService.GetIndexForLanguageKey(_settings[SettingName.LANGUAGE])},
                { SettingName.SAVE_SLOT, int.Parse(_settings[SettingName.SAVE_SLOT])}
            };
        }
    }
}