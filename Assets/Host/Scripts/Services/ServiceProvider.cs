using System;
using Gameplay.Services;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Scripts.Services.Audio;
using Host.Scripts.Services.Events;
using Host.Scripts.Services.Navigation;
using Host.Scripts.Services.SaveLoad;
using Host.Scripts.Services.Settings;
using Host.Scripts.Services.Translation;
using UniRx;

namespace Host.Scripts.Services
{
    public class ServiceProvider : IHostServiceProvider
    {
        private IEventService _eventService;
        private INavigationService _navigationService;
        private ISettingsService _settingsService;
        private IGameplayServices _gameplayServices;
        private ITranslationService _translationService;
        private ISaveLoadService _saveLoadService;
        private IAudioService _audioService;
        private bool _didLoad;

        public IAudioService AudioService() => _audioService;

        public IEventService EventService() => _eventService;

        public INavigationService NavigationService() => _navigationService;

        public ISaveLoadService SaveLoadService() => _saveLoadService;

        public ITranslationService TranslationService() => _translationService;

        public ISettingsService SettingsService() => _settingsService;

        public ServiceProvider()
        {
            CreateServices();
        }
        
        public IObservable<Unit> Load()
        {
            return _didLoad ? Observable.ReturnUnit() : InitializeServices().DoOnCompleted(NotifyInitializationComplete);
        }

        private IObservable<Unit> InitializeServices()
        {
            return _settingsService.Initialize()
                .Concat(_translationService.Initialize(), 
                    _saveLoadService.Initialize(),
                    _audioService.Initialize());
        }

        private void CreateServices()
        {
            _eventService = new EventService();
            _settingsService = new SettingsService(_eventService);
            _navigationService = new NavigationService(_eventService, new UnitySceneHelper());
            _gameplayServices = new GameplayServices();
            _saveLoadService = new SaveLoadService(_eventService, _settingsService,
                _navigationService, _gameplayServices, new SaveLoadFileUtility());
            _translationService = new TranslationService(_eventService, _settingsService);
            _audioService = new AudioService(_eventService, _settingsService);
        }

        private void NotifyInitializationComplete()
        {
            _didLoad = true;
            _eventService.SendEvent(new InitializationDoneEvent());
        }
    }
}