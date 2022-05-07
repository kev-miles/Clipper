using System;
using System.Collections.Generic;
using Host.Infrastructure.SettingsParameters;
using Host.Scripts.Services.Translation;
using UniRx;
using UnityEngine;

namespace Host.Scripts.Services.Settings
{
    public class SettingsUtility
    {
        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        private readonly Dictionary<string, string> _defaultSettings = new Dictionary<string,string>()
        {
            {SettingName.LANGUAGE, LanguageKeys.ENGLISH},
            {SettingName.MUSIC_VOLUME, "8"},
            {SettingName.SFX_VOLUME, "10"},
            {SettingName.SAVE_SLOT, "2"},
            {SettingName.AUTOSAVE_FREQUENCY, "2"}
        };
        
        public Dictionary<string, string> GetSettingsDictionary() => _settings;
        
        public IObservable<Unit> RetrieveSettings()
        {
            return Observable.Create<Unit>(emitter =>
            {
                LoadPreferences();
                emitter.OnCompleted();

                return Disposable.Empty;
            });
        }

        public void SaveSettings(Dictionary<string, string> updatedSettings)
        {
            foreach (var entry in updatedSettings)
            {
                SetPreference(entry.Key, entry.Value);
            }
            
            PlayerPrefs.Save();
        }
        
        public void ResetSettings()
        {
            _settings = _defaultSettings;
            PlayerPrefs.DeleteAll();
        }
        
        private void SetPreference(string key, string value)
        {
            if (key == SettingName.LANGUAGE)
                PlayerPrefs.SetString(key, value);
            if (key == SettingName.MUSIC_VOLUME)
                PlayerPrefs.SetInt(key, int.Parse(value));
            if (key == SettingName.SFX_VOLUME)
                PlayerPrefs.SetInt(key, int.Parse(value));
            if (key == SettingName.SAVE_SLOT)
                PlayerPrefs.SetInt(key, int.Parse(value));
            if (key == SettingName.AUTOSAVE_FREQUENCY)
                PlayerPrefs.SetInt(key, int.Parse(value));
        }

        private void LoadPreferences()
        {
            _settings[SettingName.LANGUAGE] = PlayerPrefs.HasKey(SettingName.LANGUAGE) ? 
                PlayerPrefs.GetString(SettingName.LANGUAGE) : 
                GetDefaultSettingForKey(SettingName.LANGUAGE);
            
            _settings[SettingName.MUSIC_VOLUME] = PlayerPrefs.HasKey(SettingName.MUSIC_VOLUME)
                ? PlayerPrefs.GetInt(SettingName.MUSIC_VOLUME).ToString()
                : GetDefaultSettingForKey(SettingName.MUSIC_VOLUME);
            
            _settings[SettingName.SFX_VOLUME] = PlayerPrefs.HasKey(SettingName.SFX_VOLUME) ?
                PlayerPrefs.GetInt(SettingName.SFX_VOLUME).ToString() :
                GetDefaultSettingForKey(SettingName.SFX_VOLUME);
            
            _settings[SettingName.SAVE_SLOT] = PlayerPrefs.HasKey(SettingName.SAVE_SLOT) ?
                PlayerPrefs.GetInt(SettingName.SAVE_SLOT).ToString() :
                GetDefaultSettingForKey(SettingName.SAVE_SLOT);
            
            _settings[SettingName.AUTOSAVE_FREQUENCY] = PlayerPrefs.HasKey(SettingName.AUTOSAVE_FREQUENCY) ?
                PlayerPrefs.GetInt(SettingName.AUTOSAVE_FREQUENCY).ToString() :
                GetDefaultSettingForKey(SettingName.AUTOSAVE_FREQUENCY);
        }
        
        private string GetDefaultSettingForKey(string key) => _defaultSettings[key];
    }
}