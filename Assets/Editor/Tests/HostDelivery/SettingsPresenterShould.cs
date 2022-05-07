using System.Collections.Generic;
using Host.Delivery.Screens.Settings;
using Host.Delivery.Screens.Settings.Infrastructure;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using Host.Scripts.Services.Events;
using Host.Scripts.Services.Translation;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Tests.HostDelivery
{
    public class SettingsPresenterShould
    {
        private IEventService _eventService;
        private readonly Dictionary<string, string> _settings = new() 
        {   {SettingName.LANGUAGE, LanguageKeys.ENGLISH}, 
            {SettingName.MUSIC_VOLUME, "8"}, 
            {SettingName.SFX_VOLUME, "10"}, 
            {SettingName.SAVE_SLOT, "2"},
            {SettingName.AUTOSAVE_FREQUENCY, "2"}
        };

        [SetUp]
        public void Before()
        {
            _eventService = new EventService();
        }
        
        [Test]
        public void LoadSettingsValuesAtStartup()
        {
            //Given
            var view = Substitute.For<ISettingsView>();
            var translator = Substitute.For<ITranslationService>();
            translator.GetIndexForLanguageKey(LanguageKeys.ENGLISH).Returns(0);
            var settings = Substitute.For<ISettingsService>();
            settings.GetSettings().Returns(_settings);
            var navigator = Substitute.For<INavigationService>();
            var saveLoad = Substitute.For<ISaveLoadService>();
            
            var expectedSliderValues = new Dictionary<string, float>()
            {
                {SettingName.SFX_VOLUME, 1f},
                {SettingName.MUSIC_VOLUME, 0.8f}
            };

            var audioService = Substitute.For<IAudioService>();
            audioService.GetAudioVolumes().Returns(expectedSliderValues);
            
            var presenter = new SettingsPresenter(_eventService, settings, navigator, 
                translator, saveLoad, audioService ,view);
            
            //When
            view.OnLoad();
            
            //Then
            view.Received(2).SetInitialSliders(expectedSliderValues);
            view.Received(2).SetInitialSelectors(Arg.Any<Dictionary<string, int>>());
        }

        [Test]
        public void UpdateApplicationLanguageOnLanguageSelectorUpdated()
        {
            //Given
            var view = Substitute.For<ISettingsView>();
            var translator = Substitute.For<ITranslationService>();
            var languageKeyIndex = 1;
            var expectedLanguageKey = "es";
            translator.GetLanguageKeyForIndex(languageKeyIndex).Returns(expectedLanguageKey);
            var settings = Substitute.For<ISettingsService>();
            settings.GetSettings().Returns(_settings);
            var navigator = Substitute.For<INavigationService>();
            var saveLoad = Substitute.For<ISaveLoadService>();
            var audioService = Substitute.For<IAudioService>();

            var presenter = new SettingsPresenter(_eventService, settings, navigator, 
                translator, saveLoad, audioService ,view);
            
            //When
            view.OnSettingUpdated((SettingName.LANGUAGE, languageKeyIndex));
            
            //Then
            translator.Received(1).UpdateLanguage(expectedLanguageKey);
        }
        
        [Test]
        public void UpdateSaveSlotOnSaveSlotSelectorUpdated()
        {
            //Given
            var view = Substitute.For<ISettingsView>();
            var translator = Substitute.For<ITranslationService>();
            var expectedSlot = 2;
            var settings = Substitute.For<ISettingsService>();
            settings.GetSettings().Returns(_settings);
            var navigator = Substitute.For<INavigationService>();
            var saveLoad = Substitute.For<ISaveLoadService>();
            var audioService = Substitute.For<IAudioService>();

            var presenter = new SettingsPresenter(_eventService, settings, navigator, 
                translator, saveLoad, audioService ,view);
            
            //When
            view.OnSettingUpdated((SettingName.SAVE_SLOT, expectedSlot));
            
            //Then
            saveLoad.Received(1).UpdateActiveSaveSlot(expectedSlot);
        }
        
        [Test]
        public void UpdateSoundVolumeOnSoundVolumeSliderUpdated()
        {
            //Given
            var view = Substitute.For<ISettingsView>();
            var translator = Substitute.For<ITranslationService>();
            var volume = 0.2f;
            var settings = Substitute.For<ISettingsService>();
            settings.GetSettings().Returns(_settings);
            var navigator = Substitute.For<INavigationService>();
            var saveLoad = Substitute.For<ISaveLoadService>();
            var audioService = Substitute.For<IAudioService>();

            var presenter = new SettingsPresenter(_eventService, settings, navigator, 
                translator, saveLoad, audioService ,view);
            
            //When
            view.OnSettingUpdated((SettingName.SFX_VOLUME, volume));
            
            //Then
            audioService.Received(1).UpdateSoundVolume(volume);
        }
        
        [Test]
        public void UpdateMusicVolumeOnMusicVolumeSliderUpdated()
        {
            //Given
            var view = Substitute.For<ISettingsView>();
            var translator = Substitute.For<ITranslationService>();
            var volume = 0.2f;
            var settings = Substitute.For<ISettingsService>();
            settings.GetSettings().Returns(_settings);
            var navigator = Substitute.For<INavigationService>();
            var saveLoad = Substitute.For<ISaveLoadService>();
            var audioService = Substitute.For<IAudioService>();

            var presenter = new SettingsPresenter(_eventService, settings, navigator, 
                translator, saveLoad, audioService ,view);
            
            //When
            view.OnSettingUpdated((SettingName.MUSIC_VOLUME, volume));
            
            //Then
            audioService.Received(1).UpdateMusicVolume(volume);
        }
        
        [Test]
        public void UpdateSettingsViewTextsOnLanguageChangedEvent()
        {
            //Given
            var view = Substitute.For<ISettingsView>();
            var translator = Substitute.For<ITranslationService>();
            var settings = Substitute.For<ISettingsService>();
            settings.GetSettings().Returns(_settings);
            var navigator = Substitute.For<INavigationService>();
            var saveLoad = Substitute.For<ISaveLoadService>();
            var audioService = Substitute.For<IAudioService>();

            var presenter = new SettingsPresenter(_eventService, settings, navigator, 
                translator, saveLoad, audioService ,view);
            
            //When
            _eventService.SendEvent(new LanguageChangedEvent());
            
            //Then
            view.Received(1).GetTranslaitonIDs();
            view.Received(1).SetTexts(Arg.Any<Dictionary<string, string>>());
        }
        
        [Test]
        public void SendSettingsUpdatedEventOnBackPressed()
        {
            //Given
            var view = Substitute.For<ISettingsView>();
            var events = Substitute.For<IEventService>();
            var translator = Substitute.For<ITranslationService>();
            var settings = Substitute.For<ISettingsService>();
            settings.GetSettings().Returns(_settings);
            var navigator = Substitute.For<INavigationService>();
            var saveLoad = Substitute.For<ISaveLoadService>();
            var audioService = Substitute.For<IAudioService>();

            var presenter = new SettingsPresenter(events, settings, navigator, 
                translator, saveLoad, audioService ,view);
            
            //When
            view.OnBackPressed();
            
            //Then
            events.Received(1).SendEvent(Arg.Any<SettingsUpdatedEvent>().WithParameters(_settings));
        }
    }
}