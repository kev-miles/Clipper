using Gameplay.Services.PlayerServices;

namespace Gameplay.Services
{
    public interface IGameplayServices
    {
        IPlayerService PlayerService();
    }
}