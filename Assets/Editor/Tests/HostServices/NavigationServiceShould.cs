using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Scripts.Services.Events;
using Host.Scripts.Services.Navigation;
using NSubstitute;
using NUnit.Framework;
using UniRx;

namespace Editor.Tests.HostServices
{
    public class NavigationServiceShould
    {
        [Test]
        public void ReturnNullIfNoScreenWasEverOpened()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var sceneHelper = Substitute.For<IUnitySceneHelper>();

            //When
            var navigator = new NavigationService(eventService,sceneHelper);

            //Then
            Assert.IsNull(navigator.GetCurrentActiveView());
        }
        
        [Test]
        public void AddScreenToTheStackWhenANewViewOpens()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var sceneHelper = Substitute.For<IUnitySceneHelper>();
            var navigator = new NavigationService(eventService,sceneHelper);
            var initialView = navigator.GetCurrentActiveView();
            var screenView = Substitute.For<IApplicationView>(); 
            
            //When
            navigator.OpenView(screenView);
            
            //Then
            var updatedView = navigator.GetCurrentActiveView();
            Assert.AreNotEqual(initialView, updatedView);
            Assert.AreEqual(screenView, updatedView);
        }
        
        [Test]
        public void EnableInputForNewScreen()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var sceneHelper = Substitute.For<IUnitySceneHelper>();
            var navigator = new NavigationService(eventService,sceneHelper);
            var screenView = Substitute.For<IApplicationView>(); 
            
            //When
            navigator.OpenView(screenView);
            
            //Then
            screenView.Received(1).EnableInput();
        }
        
        [Test]
        public void DisableInputForPreviousScreen()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var sceneHelper = Substitute.For<IUnitySceneHelper>();
            var navigator = new NavigationService(eventService,sceneHelper);
            var firstScreenView = Substitute.For<IApplicationView>();
            var nextScreenView = Substitute.For<IApplicationView>(); 
            
            //When
            navigator.OpenView(firstScreenView);
            navigator.OpenView(nextScreenView);
            
            //Then
            firstScreenView.Received(1).DisableInput();
        }
        
        [Test]
        public void EnableInputForPreviousScreenAfterClosingTheNewOne()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var sceneHelper = Substitute.For<IUnitySceneHelper>();
            var navigator = new NavigationService(eventService,sceneHelper);
            var firstScreenView = Substitute.For<IApplicationView>();
            var nextScreenView = Substitute.For<IApplicationView>(); 
            
            //When
            navigator.OpenView(firstScreenView);
            navigator.OpenView(nextScreenView);
            navigator.CloseView();
            
            //Then
            firstScreenView.Received(2).EnableInput();
        }

        [Test]
        public void SendEventAfterLoadingMenuScene()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var sceneHelper = Substitute.For<IUnitySceneHelper>();
            var navigator = new NavigationService(eventService,sceneHelper);
            sceneHelper.LoadMainMenu().Returns(Observable.ReturnUnit());
            
            //When
            navigator.LoadMainMenu();
            
            //Then
            eventService.Received(1).SendEvent(Arg.Any<LoadMainMenuEvent>());
        }
        
        [Test]
        public void SendEventAfterLoadingGameplayScene()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var sceneHelper = Substitute.For<IUnitySceneHelper>();
            var navigator = new NavigationService(eventService,sceneHelper);
            sceneHelper.LoadGameplayScene().Returns(Observable.ReturnUnit());
            
            //When
            navigator.LoadGameplayScene();
            
            //Then
            eventService.Received(1).SendEvent(Arg.Any<LoadGameplayEvent>());
        }
        
        [Test]
        public void SendEventAfterExitingGameplayScene()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var sceneHelper = Substitute.For<IUnitySceneHelper>();
            var navigator = new NavigationService(eventService,sceneHelper);
            sceneHelper.ExitGameplayScene().Returns(Observable.ReturnUnit());
            
            //When
            navigator.ExitGameplayScene();
            
            //Then
            eventService.Received(1).SendEvent(Arg.Any<ExitGameplayEvent>());
        }
    }
}