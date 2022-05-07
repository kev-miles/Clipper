using System.Collections.Generic;

namespace Host.Infrastructure.ApplicationEvents
{
    public struct SavedGameDeletedEvent
    {
        public string EventName => ApplicationEventNames.SAVED_GAME_DELETED;
        public Dictionary<string, int> Parameters => _parameters;
        
        private Dictionary<string, int> _parameters;
        
        public SavedGameDeletedEvent WithParameters(Dictionary<string, int> parameters)
        {
            _parameters = parameters;
            return this;
        }
    }
}