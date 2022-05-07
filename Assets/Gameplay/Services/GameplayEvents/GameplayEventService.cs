using System;
using UniRx;

namespace Gameplay.Services.GameplayEvents
{
    public class GameplayEventService
    {
        private readonly ISubject<object> _bus = new Subject<object>();

        public GameplayEventService()
        {
            _bus.Share();
        }

        public void SendEvent<T>(T e) where T : struct
        {
            _bus.OnNext(e);
        }
        
        public IObservable<T> On<T>() where T : struct
        {
            return _bus.Where(e => e is T).Select(e => (T) e);
        }
    }
}