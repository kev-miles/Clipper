using System;
using UniRx;

namespace Host.Infrastructure.Interfaces
{
    public interface IUnitySceneHelper
    {
        public IObservable<Unit> LoadMainMenu();
        public IObservable<Unit> ExitGameplayScene();
        public IObservable<Unit> LoadGameplayScene();
        public void ExitGame();
    }
}