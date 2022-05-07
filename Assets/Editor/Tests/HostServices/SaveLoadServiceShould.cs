using System.Collections.Generic;
using Gameplay.Services;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.FileManagement;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using Host.Scripts.Services.Events;
using Host.Scripts.Services.Navigation;
using Host.Scripts.Services.SaveLoad;
using Host.Scripts.Services.Settings;
using NSubstitute;
using NUnit.Framework;
using UniRx;

namespace Editor.Tests.HostServices
{
    public class SaveLoadServiceShould
    {
        [Test]
        public void SetAutoSaveFrequencyAtStartup()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var navigationService = Substitute.For<INavigationService>();
            var gameplayServices = Substitute.For<IGameplayServices>();
            var fileUtility = Substitute.For<ISaveLoadFileUtility>();
            var autosaveFrequencySetting = AutosaveSettings.Setting3;
            var saveLoadService =
                new SaveLoadServiceSeam(eventService, settingsService, navigationService, gameplayServices, fileUtility);
            fileUtility.ReadFiles().Returns(Observable.ReturnUnit());
            settingsService.GetSetting(SettingName.AUTOSAVE_FREQUENCY)
                .Returns(((int) autosaveFrequencySetting).ToString());
            settingsService.GetSetting(SettingName.SAVE_SLOT).Returns(2.ToString());

            //When
            saveLoadService.Initialize()
                .DoOnCompleted(AutosaveFrequencyIsSet)
                .Subscribe();
            
            //Then
            void AutosaveFrequencyIsSet()
            {
                Assert.AreEqual(autosaveFrequencySetting, saveLoadService.GetCurrentAutosaveSetting());
            }
        }
        
        [Test]
        public void ReturnIfTheCurrentSlotIsNewGame()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var navigationService = Substitute.For<INavigationService>();
            var gameplayServices = Substitute.For<IGameplayServices>();
            var fileUtility = Substitute.For<ISaveLoadFileUtility>();
            var slotId = 1;
            var saveLoadService =
                new SaveLoadServiceSeam(eventService, settingsService, navigationService, gameplayServices, fileUtility);
            fileUtility.SavedDataDictionary()
                .Returns(new Dictionary<int, Dictionary<string, string>>());

            //When
            saveLoadService.LoadGame(slotId);
            
            //Then
            Assert.AreEqual(saveLoadService.ActiveSaveSlot, slotId);
            Assert.IsTrue(saveLoadService.IsNewGame());
        }
        
        [Test]
        public void ReturnIfSaveSlotIsUsed()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var navigationService = Substitute.For<INavigationService>();
            var gameplayServices = Substitute.For<IGameplayServices>();
            var fileUtility = Substitute.For<ISaveLoadFileUtility>();
            var slotId = 1;
            var saveLoadService =
                new SaveLoadServiceSeam(eventService, settingsService, navigationService, gameplayServices, fileUtility);
            fileUtility.SavedDataDictionary()
                .Returns(new Dictionary<int, Dictionary<string, string>>{{slotId, new Dictionary<string, string>()}});

            //When
            var slotUsed = saveLoadService.IsSaveSlotUsed(slotId);
            
            //Then
            Assert.IsTrue(slotUsed);
        }
        
        [Test]
        public void GetAGivenSlotData()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var navigationService = Substitute.For<INavigationService>();
            var gameplayServices = Substitute.For<IGameplayServices>();
            var fileUtility = Substitute.For<ISaveLoadFileUtility>();
            var slotId = 1;
            var userName = "test_user";
            var saveLoadService =
                new SaveLoadServiceSeam(eventService, settingsService, navigationService, gameplayServices, fileUtility);
            fileUtility.SavedDataDictionary()
                .Returns(new Dictionary<int, Dictionary<string, string>>
                {   {slotId-1, new Dictionary<string, string>()},
                    {slotId, new Dictionary<string, string>{{SaveDataKeys.UserName, userName}}},
                    {slotId+1, new Dictionary<string, string>()}
                });

            //When
            var slotData = saveLoadService.GetSlotData(slotId);
            
            //Then
            Assert.AreEqual(userName, slotData[SaveDataKeys.UserName]);
        }
        
        [Test]
        public void GetTheCurrentSlotData()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var navigationService = Substitute.For<INavigationService>();
            var gameplayServices = Substitute.For<IGameplayServices>();
            var fileUtility = Substitute.For<ISaveLoadFileUtility>();
            var slotId = 1;
            var userName = "test_user";
            var saveLoadService =
                new SaveLoadServiceSeam(eventService, settingsService, navigationService, gameplayServices, fileUtility);
            fileUtility.SavedDataDictionary()
                .Returns(new Dictionary<int, Dictionary<string, string>>
                {   {slotId-1, new Dictionary<string, string>()},
                    {slotId, new Dictionary<string, string>{{SaveDataKeys.UserName, userName}}},
                    {slotId+1, new Dictionary<string, string>()}
                });

            //When
            saveLoadService.LoadGame(slotId);
            
            //Then
            Assert.AreEqual(userName, saveLoadService.GetCurrentGameData()[SaveDataKeys.UserName]);
        }
        
        [Test]
        public void ChangeAutosaveFrequency()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var navigationService = Substitute.For<INavigationService>();
            var gameplayServices = Substitute.For<IGameplayServices>();
            var fileUtility = Substitute.For<ISaveLoadFileUtility>();
            var autosaveFrequencySetting = AutosaveSettings.Setting3;
            
            var saveLoadService =
                new SaveLoadServiceSeam(eventService, settingsService, navigationService, gameplayServices, fileUtility);
            var originalAutosaveFrequency = saveLoadService.GetCurrentAutosaveSetting();
            var updatedAutosaveFrequency = AutosaveSettings.Setting3;
            
            fileUtility.ReadFiles().Returns(Observable.ReturnUnit());
            settingsService.GetSetting(SettingName.SAVE_SLOT).Returns(2.ToString());
            settingsService.GetSetting(SettingName.AUTOSAVE_FREQUENCY).Returns(((int)autosaveFrequencySetting).ToString());

            saveLoadService.Initialize()
                .DoOnCompleted(() =>
                {
                    UpdateAutosaveFrequency();
                    AutosaveFrequencyChanges();
                })
                .Subscribe();

            //When
            void UpdateAutosaveFrequency()
            {
                saveLoadService.UpdateAutosaveFrequency((int)updatedAutosaveFrequency);
            }
            
            //Then
            void AutosaveFrequencyChanges()
            {
                Assert.AreNotEqual(originalAutosaveFrequency, saveLoadService.GetCurrentAutosaveSetting());
                Assert.AreEqual(updatedAutosaveFrequency, saveLoadService.GetCurrentAutosaveSetting());
            }
        }
        
        [Test]
        public void SendEventAfterSavingGame()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var navigationService = Substitute.For<INavigationService>();
            var gameplayServices = Substitute.For<IGameplayServices>();
            var fileUtility = Substitute.For<ISaveLoadFileUtility>();
            var saveLoadService =
                new SaveLoadServiceSeam(eventService, settingsService, navigationService, gameplayServices, fileUtility);
            fileUtility.WriteFile(Arg.Any<SaveLoadObject>()).Returns(Observable.ReturnUnit());

            //When
            saveLoadService.SaveGame();
            
            //Then
            fileUtility.Received(1).WriteFile(Arg.Any<SaveLoadObject>());
            eventService.Received(1).SendEvent(Arg.Any<GameSavedEvent>());
        }
        
        [Test]
        public void SendEventAfterDeletingGame()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var navigationService = Substitute.For<INavigationService>();
            var gameplayServices = Substitute.For<IGameplayServices>();
            var fileUtility = Substitute.For<ISaveLoadFileUtility>();
            var slotToErase = 2;
            var saveLoadService =
                new SaveLoadServiceSeam(eventService, settingsService, navigationService, gameplayServices, fileUtility);
            fileUtility.DeleteFile(slotToErase).Returns(Observable.ReturnUnit());

            //When
            saveLoadService.DeleteGame(slotToErase);
            
            //Then
            fileUtility.Received(1).DeleteFile(slotToErase);
            eventService.Received(1).SendEvent(Arg.Any<SavedGameDeletedEvent>());
        }
    }

    public class SaveLoadServiceSeam : SaveLoadService
    {
        public SaveLoadServiceSeam(IEventService eventService, ISettingsService settingsService, 
            INavigationService navigationService, IGameplayServices gameplayServices, 
            ISaveLoadFileUtility fileUtility) 
            : base(eventService,settingsService,navigationService,gameplayServices,fileUtility)
        {
            
        }

        protected override SaveLoadObject BuildSaveLoadObject()
        {
            return new SaveLoadObject();
        }
    }
}