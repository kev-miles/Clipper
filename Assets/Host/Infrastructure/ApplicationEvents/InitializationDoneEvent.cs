namespace Host.Infrastructure.ApplicationEvents
{
    public struct InitializationDoneEvent
    {
        public string EventName => ApplicationEventNames.INIT_DONE;
    }
}