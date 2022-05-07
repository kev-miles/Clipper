using System;
using Host.Infrastructure.Interfaces;
using UniRx;

namespace Host.Scripts.Services.Events
{
    public class EventService : IEventService
    {
        private readonly ISubject<object> _bus = new Subject<object>();

        public EventService()
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