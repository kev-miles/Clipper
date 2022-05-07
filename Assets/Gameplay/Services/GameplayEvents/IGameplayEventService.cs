using System;

namespace Gameplay.Services.GameplayEvents
{
    public interface IGameplayEventService
    {
        public void SendEvent<T>(T e) where T : struct;
        public IObservable<T> On<T>() where T : struct;
    }
}