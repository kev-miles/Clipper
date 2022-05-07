using Host.Delivery.Screens.Credits;
using Host.Delivery.Screens.Menu;
using Host.Delivery.Screens.PresenterProvider;
using Host.Delivery.Screens.PressStart;
using Host.Delivery.Screens.Settings;
using Host.Delivery.Screens.ViewProviders;
using Host.Infrastructure.ApplicationEvents;
using Host.Scripts.Services;
using UniRx;
using UnityEngine;

namespace Host.Scripts.Application
{
    public class ApplicationRuntime : MonoBehaviour
    {
        private IViewProvider _viewProvider;
        private ServiceProvider _serviceProvider;
        private PresenterProvider _presenters;

        private void Awake()
        {
            _viewProvider = GameObject.FindGameObjectWithTag("ViewProvider").GetComponent<ViewProvider>();
            _serviceProvider = new ServiceProvider();
            _presenters = new PresenterProvider(_serviceProvider, _viewProvider);
        }

        private void Start()
        {
            _serviceProvider.Load()
                .DoOnCompleted(OnServicesLoaded)
                .Subscribe();
        }

        private void OnServicesLoaded()
        {
            //TODO: Move this to HostFlowManager
            _viewProvider.GetView<PressStartView>().OnIntroFinished += _presenters.JukeboxPresenter().Present;
            _viewProvider.GetView<PressStartView>().OnOutroFinished += _presenters.MenuPresenter().Present;

            _viewProvider.GetView<MenuView>().OnSettingsOpened += _presenters.SettingsPresenter().Present;
            _viewProvider.GetView<SettingsView>().OnOutroFinished += _presenters.MenuPresenter().Present;
            _viewProvider.GetView<MenuView>().OnCreditsOpened += _presenters.CreditsPresenter().Present;
            _viewProvider.GetView<CreditsView>().OnOutroFinished += _presenters.MenuPresenter().Present;
            
            _presenters.PressStartPresenter().Present();
        }
    }
}
