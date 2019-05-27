using System;

namespace Clarity.Engine.EventRouting
{
    public class GeneralRoutedEventSubscription : IGeneralRoutedEventSubscription
    {
        public string Name { get; }
        public Action<IRoutedEvent> HandlerAction { get; }
        public bool HandleStopped { get; }

        public GeneralRoutedEventSubscription(string name, Action<IRoutedEvent> handlerAction, bool handleStopped)
        {
            Name = name;
            HandlerAction = handlerAction;
            HandleStopped = handleStopped;
        }
    }
}