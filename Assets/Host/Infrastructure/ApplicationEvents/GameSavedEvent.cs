using System.Collections.Generic;

namespace Host.Infrastructure.ApplicationEvents
{
    public struct GameSavedEvent
    {
        public string EventName => ApplicationEventNames.GAME_SAVED;
        public Dictionary<string, int> Parameters => _parameters;
        
        private Dictionary<string, int> _parameters;
        
        public GameSavedEvent WithParameters(Dictionary<string, int> parameters)
        {
            _parameters = parameters;
            return this;
        }
    }
}