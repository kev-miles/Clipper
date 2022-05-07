using Host.Delivery.ScreenComponents.Jukebox;
using Host.Delivery.Screens.Credits;
using Host.Delivery.Screens.Menu;
using Host.Delivery.Screens.PressStart;
using Host.Delivery.Screens.Settings;
using Host.Delivery.Screens.ViewProviders;
using Host.Infrastructure.Interfaces;

namespace Host.Delivery.Screens.PresenterProvider
{
    public class PresenterProvider
    {
        private readonly IHostServiceProvider _serviceProvider;
        private readonly IViewProvider _viewProvider;
        
        private PressStartPresenter _pressStartPresenter;
        private MenuPresenter _menuPresenter;
        private SettingsPresenter _settingsPresenter;
        private CreditsPresenter _creditsPresenter;
        
        private JukeboxPresenter _jukeboxPresenter;

        public PresenterProvider(IHostServiceProvider serviceProvider, IViewProvider viewProvider)
        {
            _serviceProvider = serviceProvider;
            _viewProvider = viewProvider;
        }
        
        public PressStartPresenter PressStartPresenter() => _pressStartPresenter ?? new PressStartPresenter(_serviceProvider.EventService(),
            _serviceProvider.NavigationService(), _serviceProvider.TranslationService(),
            _viewProvider.GetView<PressStartView>());

        public MenuPresenter MenuPresenter() => _menuPresenter ?? new MenuPresenter(_serviceProvider.EventService(), 
            _serviceProvider.NavigationService(), _serviceProvider.TranslationService(), 
            _viewProvider.GetView<MenuView>());
        
        public SettingsPresenter SettingsPresenter() => _settingsPresenter ?? new SettingsPresenter(_serviceProvider.EventService(), 
            _serviceProvider.SettingsService(), _serviceProvider.NavigationService(),
            _serviceProvider.TranslationService(), _serviceProvider.SaveLoadService(), 
            _serviceProvider.AudioService(), _viewProvider.GetView<SettingsView>());
        
        public CreditsPresenter CreditsPresenter() => _creditsPresenter ?? new CreditsPresenter(_serviceProvider.EventService(), 
            _serviceProvider.NavigationService(), _serviceProvider.TranslationService(), 
            _viewProvider.GetView<CreditsView>());
        
        public JukeboxPresenter JukeboxPresenter() => _jukeboxPresenter ?? new JukeboxPresenter(_serviceProvider.EventService(), 
            _serviceProvider.TranslationService(), _viewProvider.GetView<JukeboxView>(),
            _serviceProvider.AudioService().GetAudioVolumes());
    }
}