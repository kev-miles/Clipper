using System.Collections.Generic;

namespace Host.Infrastructure.ApplicationEvents
{
    public struct LanguageChangedEvent
    {
        public string EventName => ApplicationEventNames.LANGUAGE_CHANGED;
        public Dictionary<string, object> Parameters => _parameters;
        
        private Dictionary<string, object> _parameters;
        
        public LanguageChangedEvent WithParameters(Dictionary<string, object> parameters)
        {
            _parameters = parameters;
            return this;
        }
    }
}