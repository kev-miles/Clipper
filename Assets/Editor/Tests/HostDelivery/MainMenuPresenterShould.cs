using System.Collections.Generic;
using Host.Delivery.Screens.Menu;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Scripts.Services.Events;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Tests.HostDelivery
{
    public class MainMenuPresenterShould
    {
        private EventService _eventService;

        [SetUp]
        public void Before()
        {
            _eventService = new EventService();
        }
        
        [Test]
        public void EnableInputOnMenuPresented()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<IMenuView>();

            var presenter = new MenuPresenter(eventService, navigationService, translationService, view);
            
            //When
            presenter.Present();
            
            //Then
            navigationService.OpenView(Arg.Any<IMenuView>());
        }
        
        [Test]
        public void TranslateMenuOnInitializationDone()
        {
            //Given
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<IMenuView>();

            var presenter = new MenuPresenter(_eventService, navigationService, translationService, view);
            
            //When
            _eventService.SendEvent(new InitializationDoneEvent());
            
            //Then
            translationService.TranslateAll(Arg.Any<IEnumerable<string>>());
        }
        
        [Test]
        public void TranslateMenuOnLanguageSettingsUpdated()
        {
            //Given
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<IMenuView>();

            var presenter = new MenuPresenter(_eventService, navigationService, translationService, view);
            
            //When
            _eventService.SendEvent(new LanguageChangedEvent());
            
            //Then
            translationService.TranslateAll(Arg.Any<IEnumerable<string>>());
        }
    }
}