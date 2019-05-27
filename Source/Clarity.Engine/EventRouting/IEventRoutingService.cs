using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public interface IEventRoutingService
    {
        void RegisterServiceDependency(Type dependantServiceType, Type masterServiceType);

        void Subscribe<TArgs>(string eventName, string subscriptionName, Action<IEventRoutingContext, TArgs> handlerAction, IReadOnlyList<Type> affectedServiceTypes);
        void SortSubscriptionsByDependencies(Action<EventSortingContradiction> onContradiction);

        IReadOnlyList<IEventRoutingCustomList> BuildCustomLists();
        void ApplyCustomLists(IReadOnlyList<IEventRoutingCustomList> lists, Action<string> onConflict);

        void FireEvent<TArgs>(string eventName, TArgs args);
    }
}