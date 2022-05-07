using System.Collections.Generic;
using Host.Delivery.Screens.Credits;
using Host.Delivery.Screens.Credits.Infrastructure;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Scripts.Services.Events;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Tests.HostDelivery
{
    public class CreditsPresenterShould
    {
        private EventService _eventService;

        [SetUp]
        public void Before()
        {
            _eventService = new EventService();
        }
        
        [Test]
        public void CloseViewOnBackPressed()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<ICreditsView>();

            var presenter = new CreditsPresenter(eventService, navigationService, translationService, view);
            
            //When
            view.OnBackPressed();

            //Then
            navigationService.Received(1).CloseView();
        }
        
        [Test]
        public void TranslateTextsOnPressStartEvent()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<ICreditsView>();

            var presenter = new CreditsPresenter(_eventService, navigationService, translationService, view);

            var tids = new[] {"tid_1", "tid_2", "tid_3"};
            view.GetTranslaitonIDs().Returns(tids);
            var translations = new Dictionary<string, string>();
            translationService.TranslateAll(Arg.Any<string[]>()).Returns(translations);
            
            //When
            _eventService.SendEvent(new PressStartEvent());

            //Then
            view.SetTexts(translations);
        }
        
        [Test]
        public void TranslateTextsOnLanguageChangedEvent()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<ICreditsView>();

            var presenter = new CreditsPresenter(_eventService, navigationService, translationService, view);

            var tids = new[] {"tid_1", "tid_2", "tid_3"};
            view.GetTranslaitonIDs().Returns(tids);
            var translations = new Dictionary<string, string>();
            translationService.TranslateAll(Arg.Any<string[]>()).Returns(translations);
            
            //When
            _eventService.SendEvent(new LanguageChangedEvent());

            //Then
            view.SetTexts(translations);
        }
    }
}