using System;

namespace Host.Infrastructure.Interfaces
{
    public interface IEventService
    {
        public void SendEvent<T>(T e) where T : struct;
        public IObservable<T> On<T>() where T : struct;
    }
}