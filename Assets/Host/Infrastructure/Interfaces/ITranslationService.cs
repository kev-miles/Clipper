using System;
using System.Collections.Generic;
using UniRx;

namespace Host.Infrastructure.Interfaces
{
    public interface ITranslationService
    {
        public IObservable<Unit> Initialize();
        public string Translate(string key);
        public Dictionary<string,string> TranslateAll(IEnumerable<string> keys);
        public void UpdateLanguage(string languageKey);
        public string GetCurrentLanguageKey();
        public string GetLanguageKeyForIndex(int index);
        public int GetIndexForLanguageKey(string index);
    }
}