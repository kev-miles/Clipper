using System.Collections.Generic;

namespace Gameplay.Services.PlayerServices
{
    public interface IPlayerService
    {
        public Dictionary<string, string> GetPlayerData();
    }
}