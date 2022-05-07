using System;
using Host.Infrastructure.Interfaces;
using UniRx;
using UnityEngine.SceneManagement;

namespace Host.Scripts.Services.Navigation
{
    public class UnitySceneHelper : IUnitySceneHelper

    {
        public IObservable<Unit> LoadMainMenu()
        {
            return Observable.Create<Unit>(emitter =>
            {
                SceneManager.LoadScene((int) NavigationConstants.SceneIDs.MainMenu);
                emitter.OnNext(Unit.Default);
                emitter.OnCompleted();
                
                return Disposable.Empty;
            });
        }

        public IObservable<Unit> ExitGameplayScene()
        {
            return Observable.Create<Unit>(emitter =>
            {
                SceneManager.LoadScene((int) NavigationConstants.SceneIDs.MainMenu);
                emitter.OnNext(Unit.Default);
                emitter.OnCompleted();
                
                return Disposable.Empty;
            });
        }

        public IObservable<Unit> LoadGameplayScene()
        {
            return Observable.Create<Unit>(emitter =>
            {
                SceneManager.LoadScene((int) NavigationConstants.SceneIDs.Gameplay);
                emitter.OnNext(Unit.Default);
                emitter.OnCompleted();
                
                return Disposable.Empty;
            });
        }

        public void ExitGame()
        {
            UnityEngine.Application.Quit();
        }
    }
}