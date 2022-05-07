using System.Collections.Generic;

namespace Host.Infrastructure.ApplicationEvents
{
    public struct LoadGameplayEvent
    {
        public string EventName => ApplicationEventNames.LOAD_GAMEPLAY;
        public Dictionary<string, object> Parameters => _parameters;
        
        private Dictionary<string, object> _parameters;
        
        public LoadGameplayEvent WithParameters(Dictionary<string, object> parameters)
        {
            _parameters = parameters;
            return this;
        }
    }
}