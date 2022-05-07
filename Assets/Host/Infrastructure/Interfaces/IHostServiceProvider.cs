using System;
using UniRx;

namespace Host.Infrastructure.Interfaces
{
    public interface IHostServiceProvider
    {
        IAudioService AudioService();
        IEventService EventService();
        INavigationService NavigationService(); 
        ISaveLoadService SaveLoadService();
        ITranslationService TranslationService();
        ISettingsService SettingsService();
        IObservable<Unit> Load();
    }
}