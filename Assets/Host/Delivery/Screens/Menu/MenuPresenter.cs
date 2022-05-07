using Host.Delivery.Screens.PresenterProvider;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using UniRx;

namespace Host.Delivery.Screens.Menu
{
    public class MenuPresenter : IPresenter
    {
        private readonly IEventService _eventService;
        private readonly INavigationService _navigationServince;
        private readonly ITranslationService _translationService;
        private readonly IMenuView _view;

        public MenuPresenter(IEventService eventService, INavigationService navigationServince, 
            ITranslationService translationService, IMenuView view)
        {
            _eventService = eventService;
            _navigationServince = navigationServince;
            _translationService = translationService;
            _view = view;
            SubscribeToApplicationEvents();
            TranslateTexts();
        }

        public void Present()
        {
            _view.Show();
            _navigationServince.OpenView(_view);
        }

        private void SubscribeToApplicationEvents()
        {
            _eventService.On<InitializationDoneEvent>().Subscribe(_ => TranslateTexts());
            _eventService.On<LanguageChangedEvent>().Subscribe(_ => TranslateTexts());
        }

        private void TranslateTexts()
        {
            var viewTIDs = _view.GetTranslaitonIDs();
            _view.SetTexts(_translationService.TranslateAll(viewTIDs));
        }
    }
}