using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public interface IEventRouter
    {
        Type EventType { get; }
        IEnumerable<string> GetSubscriptionNames();
        bool TrySortSubscriptionsByDependencies(IServiceEventDependencyGraph dependencyGraph, out string contradictionString);
        void ApplyCustomList(IEventRoutingCustomList list, IServiceEventDependencyGraph dependencyGraph, Action<string> onConflict);
    }

    public interface IEventRouter<TEvent> : IEventRouter
        where TEvent : IRoutedEvent
    {
        IReadOnlyList<IRoutedEventSubscription<TEvent>> Subscriptions { get; }
        void Subscribe(string subscriptionName, Action<TEvent> handlerAction, IReadOnlyList<Type> affectedServiceTypes);
    }
}