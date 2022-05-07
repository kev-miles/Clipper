using Host.Infrastructure.ApplicationEvents;
using Host.Scripts.Services;
using NUnit.Framework;
using UniRx;

namespace Editor.Tests.HostServices
{
    public class ServiceProviderShould
    {
        [Test]
        public void InitializeServices()
        {
            //Given
            var provider = new ServiceProvider();
            
            //When
            provider.Load()
                .DoOnCompleted(ServicesAreInitialized)
                .Subscribe();
                
            //Then
            void ServicesAreInitialized()
            {
                Assert.IsNotNull(provider.EventService());
                Assert.IsNotNull(provider.SettingsService());
                Assert.IsNotNull(provider.TranslationService());
                Assert.IsNotNull(provider.NavigationService());
                Assert.IsNotNull(provider.SaveLoadService());
                Assert.IsNotNull(provider.AudioService());
            }
        }
        
        [Test]
        public void NotifyWhenInitializationIsDone()
        {
            //Given
            var provider = new ServiceProvider();
            var eventSent = false;
            
            //When
            provider.Load().Do(_ =>
            {
                if (provider.EventService() != null)
                {
                    provider.EventService().On<InitializationDoneEvent>().Subscribe(_ => eventSent = true);
                }
            })
                .DoOnCompleted(InitializationDoneEventIsSent)
                .Subscribe();
                
            //Then
            void InitializationDoneEventIsSent()
            {
                Assert.IsTrue(eventSent);
            }
        }
    }
}