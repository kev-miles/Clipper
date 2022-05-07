using Host.Delivery.Screens.PresenterProvider;
using Host.Delivery.Screens.PressStart.Infrastructure;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using UniRx;

namespace Host.Delivery.Screens.PressStart
{
    public class PressStartPresenter : IPresenter
    {
        private readonly IEventService _eventService;
        private readonly INavigationService _navigationService;
        private readonly ITranslationService _translationService;
        private readonly IPressStartView _view;

        public PressStartPresenter(IEventService eventService, INavigationService navigationService,
            ITranslationService translationService, IPressStartView view)
        {
            _eventService = eventService;
            _navigationService = navigationService;
            _translationService = translationService;
            _view = view;
            SubscribeToApplicationEvents();
            SubscribeToViewEvents();
            TranslateTexts();
        }

        public void Present()
        {
            _view.Show();
        }

        private void SubscribeToApplicationEvents()
        {
            _eventService.On<LanguageChangedEvent>().Subscribe(_ => TranslateTexts());
        }
        
        private void SubscribeToViewEvents()
        {
            _view.OnIntroFinished += OpenView;
            _view.OnKeyPressed += SendPressStartEvent;
        }

        private void OpenView()
        {
            _navigationService.OpenView(_view);
        }

        private void SendPressStartEvent()
        {
            _eventService.SendEvent(new PressStartEvent());
        }

        private void TranslateTexts()
        {
            _view.SetTexts(_translationService.TranslateAll(_view.GetTranslaitonIDs()));
        }
    }
}