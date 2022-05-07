using System;
using System.Collections.Generic;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using UniRx;

namespace Host.Scripts.Services.Translation
{
    public class TranslationService : ITranslationService
    {
        private readonly IEventService _eventService;
        private readonly CsvLoader _fileLoader;
        private Dictionary<string, string> _localizationDictionary;
        private readonly ISettingsService _settingsService;

        private string _currentLanguageKey; 

        public TranslationService(IEventService eventService, ISettingsService settingsService)
        {
            _eventService = eventService;
            _settingsService = settingsService;
            _fileLoader = new CsvLoader();
        }

        public IObservable<Unit> Initialize()
        {
            return _fileLoader.LoadFiles().DoOnCompleted(() =>
            {
                UpdateLanguage(_settingsService.GetSetting(SettingName.LANGUAGE));
            });
        }

        public string Translate(string key)
        {
            return _localizationDictionary.ContainsKey(key) ? _localizationDictionary[key] : key;
        }

        public Dictionary<string, string> TranslateAll(IEnumerable<string> keys)
        {
            var translatedKeys = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                if (!translatedKeys.ContainsKey(key))
                {
                    translatedKeys.Add(key, Translate(key));
                }
            }
            return translatedKeys;
        }

        public void UpdateLanguage(string languageKey)
        {
            if (_currentLanguageKey == languageKey) return;
            _currentLanguageKey = languageKey;
            _localizationDictionary = _fileLoader.GetLocalizationKeysForLanguage(languageKey);
            _eventService.SendEvent(new LanguageChangedEvent());
        }

        public string GetCurrentLanguageKey()
        {
            return _currentLanguageKey;
        }

        public string GetLanguageKeyForIndex(int index) => 
            _fileLoader.LanguageKeys[index % _fileLoader.LanguageKeys.Length];

        public int GetIndexForLanguageKey(string key) =>
            Array.IndexOf(_fileLoader.LanguageKeys,key);
    }
}
