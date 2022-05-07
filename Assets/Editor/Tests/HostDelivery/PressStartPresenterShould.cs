using System.Collections.Generic;
using Host.Delivery.Screens.PressStart;
using Host.Delivery.Screens.PressStart.Infrastructure;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Tests.HostDelivery
{
    public class PressStartPresenterShould
    {
        [Test]
        public void TranslateScreenAtStartUp()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<IPressStartView>();

            //When
            var presenter = new PressStartPresenter(eventService, navigationService, translationService, view);

            //Then
            translationService.Received(1).TranslateAll(Arg.Any<IEnumerable<string>>());
        }
        
        [Test]
        public void PresentScreen()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<IPressStartView>();
            
            var presenter = new PressStartPresenter(eventService, navigationService, translationService, view);

            //When
            presenter.Present();

            //Then
            view.Received(1).Show();
        }

        [Test]
        public void EnableInputOnIntroComplete()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<IPressStartView>();

            var presenter = new PressStartPresenter(eventService, navigationService, translationService, view);
            
            //When
            view.OnIntroFinished();

            //Then
            navigationService.Received(1).OpenView(Arg.Any<IPressStartView>());
        }
        
        [Test]
        public void SendPressStartEventOnButtonPressed()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var navigationService = Substitute.For<INavigationService>();
            var translationService = Substitute.For<ITranslationService>();
            var view = Substitute.For<IPressStartView>();

            var presenter = new PressStartPresenter(eventService, navigationService, translationService, view);
            
            //When
            view.OnKeyPressed();

            //Then
            eventService.Received(1).SendEvent(Arg.Any<PressStartEvent>());
        }
    }
}