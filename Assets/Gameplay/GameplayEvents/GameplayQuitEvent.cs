using System.Collections.Generic;

namespace Gameplay.GameplayEvents
{
    public struct GameplayQuitEvent
    {
        public string EventName() => GameplayEventNames.QUIT_GAMEPLAY;
        public Dictionary<string, object> Parameters() => _parameters;
        
        private Dictionary<string, object> _parameters;
        
        public GameplayQuitEvent WithParameters(Dictionary<string, object> parameters)
        {
            _parameters = parameters;
            return this;
        }
    }
}