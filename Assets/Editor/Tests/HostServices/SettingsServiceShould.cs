using System.Collections.Generic;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using Host.Scripts.Services.Events;
using Host.Scripts.Services.Settings;
using Host.Scripts.Services.Translation;
using NSubstitute;
using NUnit.Framework;
using UniRx;
using UnityEngine.EventSystems;

namespace Editor.Tests.HostServices
{
    public class SettingsServiceShould
    {
        public static Dictionary<string, string> settings;

        [SetUp]
        public void Before()
        {
            settings = new Dictionary<string, string>()
            {
                {SettingName.LANGUAGE, LanguageKeys.ENGLISH},
                {SettingName.MUSIC_VOLUME, "8"},
                {SettingName.SFX_VOLUME, "10"},
                {SettingName.SAVE_SLOT, "2"},
                {SettingName.AUTOSAVE_FREQUENCY, "2"}
            };
        }
        
        [Test]
        public void RetrieveSettings()
        {
            //Given
            var eventService = Substitute.For<EventService>();
            var settingsService = new SettingsService(eventService);

            //When
            settingsService.Initialize()
                .DoOnCompleted(SettingsAreReturned)
                .Subscribe();
            
            //Then
            void SettingsAreReturned()
            {
                var settingsReturned = settingsService.GetSettings();
                foreach (var entry in settings)
                {
                    Assert.IsNotNull(settingsReturned[entry.Key]);
                }
            }
        }

        [Test]
        public void UpdateSettingsOnSettingsChangedEvent()
        {
            //Given
            var eventService = new EventService();
            var settingsChangedEvent = MakeSettingsUpdatedEvent();
            var expectedSettingValue = LanguageKeys.SPANISH;
            var settingsService = new SettingsServiceSeam(eventService);
                
            settingsService.Initialize()
                .DoOnCompleted(() =>
                {
                    ChangeSettings();
                    SettingsAreUpdated();
                })
                .Subscribe();
            
            //When
            void ChangeSettings()
            {
                eventService.SendEvent(settingsChangedEvent);
            }
            
            //Then
            void SettingsAreUpdated()
            {
                Assert.AreEqual(expectedSettingValue, settings[SettingName.LANGUAGE]);
            }
        }

        private SettingsUpdatedEvent MakeSettingsUpdatedEvent() 
            => new SettingsUpdatedEvent().WithParameters(new Dictionary<string, string>() {{SettingName.LANGUAGE, LanguageKeys.SPANISH}});

        private class SettingsServiceSeam : SettingsService
        {
            public SettingsServiceSeam(IEventService eventService) : base(eventService)
            { }

            protected override void UpdateSettings(Dictionary<string, string> newSettings)
            {
                foreach (var entry in newSettings)
                {
                    settings[entry.Key] = entry.Value;
                }
            }
        }
    }
}