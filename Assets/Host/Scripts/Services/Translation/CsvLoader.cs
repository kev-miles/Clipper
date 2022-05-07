using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;

namespace Host.Scripts.Services.Translation
{
    public class CsvLoader
    {
        private readonly string[] _languageKeys = {"en", "es"};
        private readonly char _terminator = '\n';
        private readonly char _entryMark = '"';

        private readonly Dictionary<string, Dictionary<string, string>> _localizationDirectory =
            new Dictionary<string, Dictionary<string, string>>();

        public string[] LanguageKeys => _languageKeys;
        
        public Dictionary<string, string> GetLocalizationKeysForLanguage(string languageKey = "en")
        {
            return _localizationDirectory.ContainsKey(languageKey)
                ? _localizationDirectory[languageKey]
                : new Dictionary<string, string>();
        }
        
        public IObservable<Unit> LoadFiles()
        {
            return Observable.Create<Unit>(emitter =>
            {
                foreach (var key in _languageKeys)
                {
                    var file = Resources.Load<TextAsset>($"Localization/{key}");
                    GetLanguageDictionary(key, file);
                    emitter.OnNext(Unit.Default);
                }
                
                emitter.OnCompleted();

                return Disposable.Empty;
            });
        }
        
        private void GetLanguageDictionary(string key, TextAsset file)
        {
            var lines = file.text.Split(_terminator);
             Regex parser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
             _localizationDirectory[key] = ParseFile(lines, parser);
        }

        private Dictionary<string, string> ParseFile(string[] lines, Regex parser)
        {
            var dictionary = new Dictionary<string, string>();
            for(var i=1; i<lines.Length; i++)
            {
                var line = lines[i];
                var fields = parser.Split(line);

                for (var j = 0; j < fields.Length; j++)
                {
                    fields[j] = fields[j].TrimStart(' ', _entryMark);
                    fields[j] = fields[j].TrimEnd('\r', _entryMark);
                }

                var localizationKey = fields.FirstOrDefault();
                var localizationValue = fields.LastOrDefault();

                if(!dictionary.ContainsKey(localizationKey))
                    dictionary.Add(localizationKey, localizationValue);
            }

            return dictionary;
        }
    }
}