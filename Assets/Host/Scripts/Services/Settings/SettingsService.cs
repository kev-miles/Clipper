using System;
using System.Collections.Generic;
using System.Linq;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Scripts.Services.Events;
using UniRx;

namespace Host.Scripts.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly IEventService _eventsService;
        private readonly SettingsUtility _settingsUtility;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public SettingsService(IEventService eventService)
        {
            _eventsService = eventService;
            _settingsUtility = new SettingsUtility();
            SubscribeToApplicationEvents();
        }

        public IObservable<Unit> Initialize() => _settingsUtility.RetrieveSettings();

        public string GetSetting(string settingKey) => _settingsUtility.GetSettingsDictionary()[settingKey];
        
        public Dictionary<string, string> GetSettings() => _settingsUtility.GetSettingsDictionary();

        protected virtual void UpdateSettings(Dictionary<string, string> newSettings)
        {
            var settingsToUpdate = _settingsUtility.GetSettingsDictionary();
            foreach (var entry in newSettings.ToList())
            {
                settingsToUpdate[entry.Key] = entry.Value;
            }
            _settingsUtility.SaveSettings(settingsToUpdate);
        }
        
        private void ResetSettings() => _settingsUtility.ResetSettings();
        
        private void SubscribeToApplicationEvents()
        {
            _eventsService.On<SettingsUpdatedEvent>().Subscribe(e => UpdateSettings(e.Parameters)).AddTo(_disposable);
        }
    }
}