using System.Collections.Generic;

namespace Host.Infrastructure.ApplicationEvents
{
    public struct ExitGameplayEvent
    {
        public string EventName() => ApplicationEventNames.EXIT_GAMEPLAY;
        public Dictionary<string, object> Parameters() => _parameters;
        
        private Dictionary<string, object> _parameters;
        
        public ExitGameplayEvent WithParameters(Dictionary<string, object> parameters)
        {
            _parameters = parameters;
            return this;
        }
    }
}