using System.Collections.Generic;

namespace Host.Infrastructure.ApplicationEvents
{
    public struct SettingsUpdatedEvent
    {
        public string EventName => ApplicationEventNames.SETTINGS_UPDATED;
        public Dictionary<string, string> Parameters => _parameters;
        
        private Dictionary<string, string> _parameters;
        
        public SettingsUpdatedEvent WithParameters(Dictionary<string, string> parameters)
        {
            _parameters = parameters;
            return this;
        }
    }
}