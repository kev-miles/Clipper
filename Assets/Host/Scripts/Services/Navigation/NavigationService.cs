using System.Collections.Generic;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Scripts.Services.Events;
using JetBrains.Annotations;
using UniRx;

namespace Host.Scripts.Services.Navigation
{
    public class NavigationService : INavigationService 
    {
        private readonly Stack<IApplicationView> _screenStack = new Stack<IApplicationView>();
        private readonly IEventService _eventService;
        private readonly IUnitySceneHelper _sceneHelper;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly Subject<IApplicationView> _onViewClosed = new Subject<IApplicationView>();
        
        public NavigationService(IEventService eventService, IUnitySceneHelper sceneHelper)
        {
            _eventService = eventService;
            _sceneHelper = sceneHelper;
        }

        [CanBeNull]
        public IApplicationView GetCurrentActiveView() =>
                _screenStack.Count > 0
                ? _screenStack.Peek()
                : null;
        

        public void OpenView(IApplicationView view)
        {
            foreach (var screen in _screenStack)
            {
                screen.DisableInput();
            }
            
            view.EnableInput();
            _screenStack.Push(view);
        }

        public void CloseView()
        {
            _onViewClosed.OnNext(_screenStack.Peek());
            _screenStack.Pop();
            if(_screenStack.Count > 0) _screenStack.Peek().EnableInput();
        }
        
        public void LoadMainMenu()
        {
            _sceneHelper.LoadMainMenu()
                .Subscribe(_ => _eventService.SendEvent(new LoadMainMenuEvent())).AddTo(_disposable);
        }

        public void ExitGameplayScene()
        {
            _screenStack.Clear();
            _sceneHelper.ExitGameplayScene()
                .Subscribe(_ => _eventService.SendEvent(new ExitGameplayEvent())).AddTo(_disposable);
        }

        public void LoadGameplayScene()
        {
            _screenStack.Clear();
            _sceneHelper.LoadGameplayScene()
                .Subscribe(_ => _eventService.SendEvent(new LoadGameplayEvent())).AddTo(_disposable);
        }

        public void ExitGame()
        {
            _disposable.Dispose();
            _sceneHelper.ExitGame();
        }
    }
}
