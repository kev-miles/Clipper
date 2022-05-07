using System;
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

namespace Editor.Tests.HostServices.Translation
{
    public class TranslatorServiceShould
    {
        [Test]
        public void InitializeWithLanguageFromSettings()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            settingsService.GetSetting(SettingName.LANGUAGE).Returns(LanguageKeys.ENGLISH);

            var translator = new TranslationService(eventService, settingsService);

            //When
            translator.Initialize()
                .DoOnCompleted(LanguageKeyIsNotNull)
                .Subscribe();

            //Then
            void LanguageKeyIsNotNull()
            {
                Assert.AreEqual(LanguageKeys.ENGLISH, translator.GetCurrentLanguageKey());
            }
        }

        [Test]
        public void SendEventOnLanguageChanged()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            settingsService.GetSetting(SettingName.LANGUAGE).Returns(LanguageKeys.ENGLISH);

            var translator = new TranslationService(eventService, settingsService);

            //When
            translator.Initialize()
                .DoOnCompleted(() =>
                {
                    ChangeLanguage();
                    TwoEventsWereSent();
                })
                .Subscribe();

            void ChangeLanguage()
            {
                translator.UpdateLanguage(LanguageKeys.SPANISH);
            }
            
            //Then
            void TwoEventsWereSent()
            {
                eventService.Received(2).SendEvent(Arg.Any<LanguageChangedEvent>());
            }
        }

        [Test]
        public void NotSendEventIfLanguageIsTheSame()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            settingsService.GetSetting(SettingName.LANGUAGE).Returns(LanguageKeys.ENGLISH);

            var translator = new TranslationService(eventService, settingsService);

            //When
            translator.Initialize()
                .DoOnCompleted(() =>
                {
                    ChangeLanguage();
                    OneEventWasSent();
                })
                .Subscribe();

            void ChangeLanguage()
            {
                translator.UpdateLanguage(LanguageKeys.ENGLISH);
            }
            
            //Then
            void OneEventWasSent()
            {
                eventService.Received(1).SendEvent(Arg.Any<LanguageChangedEvent>());
            }
        }
        
        [TestCase("tid_accept", "Accept")]
        [TestCase("tid_cancel", "Cancel")]
        [TestCase("tid_exit_game", "Exit")]
        [TestCase("tid_press_start", "Press Start")]
        [TestCase("tid_sfx_volume", "Sound Effects Volume")]
        public void ReturnTheTranslatedKeyIfAvailable(string keyToTranslate, string expectedTranslation)
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            settingsService.GetSetting(SettingName.LANGUAGE).Returns(LanguageKeys.ENGLISH);
            
            var translator = new TranslationService(eventService, settingsService);
            var localizedString = String.Empty;

            translator.Initialize()
                .DoOnCompleted(() =>
                {
                    TranslateKey();
                    TextIsTranslated();
                })
                .Subscribe();
            
            //When
            void TranslateKey()
            {
                localizedString = translator.Translate(keyToTranslate);
            }
            
            //Then
            void TextIsTranslated()
            {
                Assert.AreEqual(expectedTranslation, localizedString);
            }
        }
        
        [Test]
        public void ReturnTheKeyIfTranslationIsNotAvailable()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            settingsService.GetSetting(SettingName.LANGUAGE).Returns(LanguageKeys.ENGLISH);
            
            var translator = new TranslationService(eventService, settingsService);
            var keyToTranslate = "this_translation_is_not_available";
            var localizedString = String.Empty;

            translator.Initialize()
                .DoOnCompleted(() =>
                {
                    TranslateKey();
                    KeyIsReturned();
                })
                .Subscribe();
            
            //When
            void TranslateKey()
            {
                localizedString = translator.Translate(keyToTranslate);
            }
            
            //Then
            void KeyIsReturned()
            {
                Assert.AreEqual(keyToTranslate, localizedString);
            }
        }

        [Test]
        public void TranslateAllTextsInAGivenArray()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            settingsService.GetSetting(SettingName.LANGUAGE).Returns(LanguageKeys.ENGLISH);
            
            var translator = new TranslationService(eventService, settingsService);
            var keysToTranslate = new[] {"tid_accept", "tid_cancel", "tid_exit_game"};
            var translatedKeys = new Dictionary<string, string>();
            var expectedtKeys = new Dictionary<string, string>
            {
                {"tid_accept", "Accept"},
                {"tid_cancel", "Cancel"},
                {"tid_exit_game", "Exit"},
            };

            translator.Initialize()
                .DoOnCompleted(() =>
                {
                    TranslateKeys();
                    KeysAreTranslated();
                })
                .Subscribe();
            
            //When
            void TranslateKeys()
            {
                translatedKeys = translator.TranslateAll(keysToTranslate);
            }
            
            //Then
            void KeysAreTranslated()
            {
                foreach (var entry in translatedKeys)
                {
                    Assert.AreEqual(expectedtKeys[entry.Key], entry.Value);
                }
            }
        }

        [Test]
        public void ReturnALanguageKeyForAGivenIndex()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            settingsService.GetSetting(SettingName.LANGUAGE).Returns(LanguageKeys.ENGLISH);
            var expectedKey = "es";

            var translator = new TranslationService(eventService, settingsService);
            
            //When
            var languageKey = translator.GetLanguageKeyForIndex(1);

            //Then
            Assert.AreEqual(expectedKey, languageKey);
        }
        
        [Test]
        public void NotFailIfLanguageKeyIndexIsOutOfBounds()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            settingsService.GetSetting(SettingName.LANGUAGE).Returns(LanguageKeys.ENGLISH);
            var translator = new TranslationService(eventService, settingsService);
            
            //When
            var languageKey = translator.GetLanguageKeyForIndex(40);
        }
    }
}
