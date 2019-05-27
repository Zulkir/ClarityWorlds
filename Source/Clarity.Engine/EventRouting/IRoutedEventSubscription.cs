using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public interface IRoutedEventSubscription
    {
        string Name { get; }
        IReadOnlyList<Type> AffectedServiceTypes { get; }
    }

    public interface IRoutedEventSubscription<in TArgs> : IRoutedEventSubscription
    {
        Action<IEventRoutingContext, TArgs> HandlerAction { get; }
    }
}