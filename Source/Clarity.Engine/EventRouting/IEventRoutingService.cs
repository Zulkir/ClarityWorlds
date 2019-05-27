using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public interface IEventRoutingService
    {
        void RegisterServiceDependency(Type dependantServiceType, Type masterServiceType);

        void Subscribe<TEvent>(string subscriptionName, Action<TEvent> handlerAction, IReadOnlyList<Type> affectedServiceTypes) where TEvent : IRoutedEvent;
        void SubscribeToAllBefore(string subscriptionName, Action<IRoutedEvent> handlerAction, bool handleStopped);
        void SubscribeToAllAfter(string subscriptionName, Action<IRoutedEvent> handlerAction, bool handleStopped);
        void SortSubscriptionsByDependencies(Action<EventSortingContradiction> onContradiction);

        IReadOnlyList<IEventRoutingCustomList> BuildCustomLists();
        void ApplyCustomLists(IReadOnlyList<IEventRoutingCustomList> lists, Action<string> onConflict);

        void FireEvent<TEvent>(TEvent ev) where TEvent : IRoutedEvent;
    }
}