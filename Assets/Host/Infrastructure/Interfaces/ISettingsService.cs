using System;
using System.Collections.Generic;
using UniRx;

namespace Host.Infrastructure.Interfaces
{
    public interface ISettingsService
    {
        public IObservable<Unit> Initialize();
        public string GetSetting(string settingKey);
        public Dictionary<string, string> GetSettings();
    }
}