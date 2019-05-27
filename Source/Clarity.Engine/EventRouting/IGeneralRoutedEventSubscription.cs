using System;

namespace Clarity.Engine.EventRouting
{
    public interface IGeneralRoutedEventSubscription
    {
        string Name { get; }
        Action<IRoutedEvent> HandlerAction { get; }
        bool HandleStopped { get; }
    }
}