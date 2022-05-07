using System;
using System.Collections.Generic;
using Gameplay.Services;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.FileManagement;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using Host.Scripts.Services.Events;
using Host.Scripts.Services.Navigation;
using Host.Scripts.Services.Settings;
using UniRx;

namespace Host.Scripts.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        public int ActiveSaveSlot => _activeSaveSlot;
        
        private readonly IEventService _eventService;
        private readonly INavigationService _navigator;
        private readonly ISettingsService _settingsService;
        private readonly ISaveLoadFileUtility _saveLoadUtility;
        private readonly IGameplayServices _gameplayServices;
        private IDisposable _disposable;

        private AutosaveSettings _currentAutosaveFrequency;
        public string[] AutosaveOptionKeys { get; } = new[]
        {
            "tid_autosave_1",
            "tid_autosave_2",
            "tid_autosave_3",
            "tid_autosave_4"
        };

        private int _activeSaveSlot = -1;

        public SaveLoadService(IEventService eventService, ISettingsService settingsService, 
            INavigationService navigationService, IGameplayServices gameplayServices, 
            ISaveLoadFileUtility fileUtility)
        {
            _eventService = eventService;
            _settingsService = settingsService;
            _navigator = navigationService;
            _gameplayServices = gameplayServices;
            _saveLoadUtility = fileUtility;
            SubscribeToApplicationEvents();
        }

        public IObservable<Unit> Initialize()
        {
            return _saveLoadUtility.ReadFiles().DoOnCompleted(() =>
            {
                UpdateActiveSaveSlot(int.Parse(_settingsService.GetSetting(SettingName.SAVE_SLOT)));
                UpdateAutosaveFrequency(int.Parse(_settingsService.GetSetting(SettingName.AUTOSAVE_FREQUENCY)));
            });
        }

        public AutosaveSettings GetCurrentAutosaveSetting() => _currentAutosaveFrequency;

        public bool IsSaveSlotUsed(int slotId)
        {
            return _saveLoadUtility.SavedDataDictionary().ContainsKey(slotId);
        }

        public bool IsNewGame()
        {
            return !IsSaveSlotUsed(_activeSaveSlot);
        }

        public Dictionary<string, string> GetCurrentGameData()
        {
            return _saveLoadUtility.SavedDataDictionary()[_activeSaveSlot];
        }

        public Dictionary<string, string> GetSlotData(int slotId)
        {
            return _saveLoadUtility.SavedDataDictionary()[slotId];
        }
        
        public void SaveGame()
        {
            _disposable = _saveLoadUtility.WriteFile(BuildSaveLoadObject()).Subscribe(_ =>
            {
                _eventService.SendEvent(new GameSavedEvent().WithParameters(BuildEventParameters(_activeSaveSlot)));
            });
        }

        public void LoadGame(int slotId)
        {
            //TODO: Load data from save slot
            UpdateActiveSaveSlot(slotId);
            _navigator.LoadGameplayScene();
        } 
        
        public void DeleteGame(int slotId)
        {
            _disposable = _saveLoadUtility.DeleteFile(slotId).Subscribe(_ =>
            {
                _eventService.SendEvent(new SavedGameDeletedEvent().WithParameters(BuildEventParameters(slotId)));
            });
        }

        public void UpdateAutosaveFrequency(int setting)
        {
            _currentAutosaveFrequency = (AutosaveSettings) setting;
        }
        
        public void UpdateActiveSaveSlot(int slot)
        {
            _activeSaveSlot = slot;
        }
        
        private void SubscribeToApplicationEvents()
        {
            _eventService.On<ExitGameplayEvent>().Subscribe(_ => _saveLoadUtility.ReadFile(_activeSaveSlot).Subscribe());
        }

        protected virtual SaveLoadObject BuildSaveLoadObject() => 
            SaveLoadObject.Create(_activeSaveSlot, _gameplayServices.PlayerService().GetPlayerData());
        
        private Dictionary<string, int> BuildEventParameters(int slotId) =>
            new Dictionary<string, int>() {{SaveDataKeys.SlotId, slotId}};
    }
}
