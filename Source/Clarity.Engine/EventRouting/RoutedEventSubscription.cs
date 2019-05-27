using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public class RoutedEventSubscription<TEvent> : IRoutedEventSubscription<TEvent>
    {
        public string Name { get; }
        public Action<TEvent> HandlerAction { get; }
        public IReadOnlyList<Type> AffectedServiceTypes { get; }

        public RoutedEventSubscription(string name, Action<TEvent> handlerAction, IReadOnlyList<Type> affectedServiceTypes)
        {
            Name = name;
            HandlerAction = handlerAction;
            AffectedServiceTypes = affectedServiceTypes;
        }
    }
}