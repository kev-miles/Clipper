using Gameplay.Services.PlayerServices;

namespace Gameplay.Services
{
    public class GameplayServices : IGameplayServices
    {
        //TODO: Implement this
        public IPlayerService PlayerService()
        {
            return new PlayerService();
        }
    }
}