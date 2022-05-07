using System;
using System.Collections.Generic;
using Host.Infrastructure.FileManagement;
using UniRx;

namespace Host.Infrastructure.Interfaces
{
    public interface ISaveLoadService
    {
        IObservable<Unit> Initialize();
        AutosaveSettings GetCurrentAutosaveSetting();
        bool IsSaveSlotUsed(int slotId);
        bool IsNewGame();
        Dictionary<string, string> GetCurrentGameData();
        Dictionary<string, string> GetSlotData(int slotId);
        void SaveGame();
        void LoadGame(int slotId);
        void DeleteGame(int slotId);
        void UpdateAutosaveFrequency(int setting);
        void UpdateActiveSaveSlot(int slot);
    }
}