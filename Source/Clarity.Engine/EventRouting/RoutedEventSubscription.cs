using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public class RoutedEventSubscription<TArgs> : IRoutedEventSubscription<TArgs>
    {
        public string Name { get; }
        public Action<IEventRoutingContext, TArgs> HandlerAction { get; }
        public IReadOnlyList<Type> AffectedServiceTypes { get; }

        public RoutedEventSubscription(string name, Action<IEventRoutingContext, TArgs> handlerAction, IReadOnlyList<Type> affectedServiceTypes)
        {
            Name = name;
            HandlerAction = handlerAction;
            AffectedServiceTypes = affectedServiceTypes;
        }
    }
}