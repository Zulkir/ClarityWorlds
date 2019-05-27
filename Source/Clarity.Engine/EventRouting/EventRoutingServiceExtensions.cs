using System;

namespace Clarity.Engine.EventRouting
{
    public static class EventRoutingServiceExtensions
    {
        public static void Subscribe<TEvent>(this IEventRoutingService eventRoutingService, Type serviceType, string methodName, Action<TEvent> handlerAction) 
            where TEvent : IRoutedEvent
        {
            eventRoutingService.Subscribe($"{serviceType.FullName}.{methodName}", handlerAction, new []{serviceType});
        }
    }
}