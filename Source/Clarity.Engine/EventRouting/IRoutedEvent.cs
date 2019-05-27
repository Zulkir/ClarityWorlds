using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public interface IRoutedEvent
    {
        string Name { get; }
        IEnumerable<string> GetSubscriptionNames();
        bool TrySortSubscriptionsByDependencies(IServiceEventDependencyGraph dependencyGraph, out string contradictionString);
        void ApplyCustomList(IEventRoutingCustomList list, IServiceEventDependencyGraph dependencyGraph, Action<string> onConflict);
    }

    public interface IRoutedEvent<TArgs> : IRoutedEvent
    {
        IReadOnlyList<IRoutedEventSubscription<TArgs>> Subscriptions { get; }
        void Subscribe(string subscriptionName, Action<IEventRoutingContext, TArgs> handlerAction, IReadOnlyList<Type> affectedServiceTypes);
    }
}