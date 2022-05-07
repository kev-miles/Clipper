using Host.Delivery.Screens.Credits.Infrastructure;
using Host.Delivery.Screens.PresenterProvider;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using UniRx;

namespace Host.Delivery.Screens.Credits
{
    public class CreditsPresenter : IPresenter

    {
        private readonly IEventService _eventService;
        private readonly ITranslationService _translationService;
        private readonly INavigationService _navigationService;
        private readonly ICreditsView _view;

        public CreditsPresenter(IEventService eventService, INavigationService navigationService,
            ITranslationService translationService, ICreditsView view)
        {
            _eventService = eventService;
            _navigationService = navigationService;
            _translationService = translationService;
            _view = view;
            SubscribeToViewEvents();
            SubscribeToApplicationEvents();
        }

        public void Present()
        {
            _view.Show();
            _navigationService.OpenView(_view);
        }
        
        private void SubscribeToViewEvents()
        {
            _view.OnIntroFinished += () => {};
            _view.OnBackPressed += _navigationService.CloseView;
        }

        private void SubscribeToApplicationEvents()
        {
            _eventService.On<PressStartEvent>().Subscribe(_ => TranslateTexts());
            _eventService.On<LanguageChangedEvent>().Subscribe(_ => TranslateTexts());
        }

        private void TranslateTexts()
        {
            var tids = _translationService.TranslateAll(_view.GetTranslaitonIDs());
            _view.SetTexts(tids);
        }
    }
}