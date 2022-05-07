using System.Collections.Generic;

namespace Host.Infrastructure.ApplicationEvents
{
    public struct AudioSettingsChangedEvent
    {
        public string EventName => ApplicationEventNames.AUDIO_SETTINGS_CHANGED;
        public Dictionary<string, float> Parameters => _parameters;
        
        private Dictionary<string, float> _parameters;
        
        public AudioSettingsChangedEvent WithParameters(Dictionary<string, float> parameters)
        {
            _parameters = parameters;
            return this;
        }
    }
}