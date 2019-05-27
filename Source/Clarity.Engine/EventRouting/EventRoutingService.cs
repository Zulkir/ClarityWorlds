using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Engine.EventRouting
{
    public class EventRoutingService : IEventRoutingService
    {
        private readonly IServiceEventDependencyGraph dependencyGraph;
        private readonly Dictionary<Type, IEventRouter> routers;
        private readonly Dictionary<string, IGeneralRoutedEventSubscription> generalSubscriptionsBefore;
        private readonly Dictionary<string, IGeneralRoutedEventSubscription> generalSubscriptionsAfter;

        public EventRoutingService()
        {
            dependencyGraph = new ServiceEventDependencyGraph();
            routers = new Dictionary<Type, IEventRouter>();
            generalSubscriptionsBefore = new Dictionary<string, IGeneralRoutedEventSubscription>();
            generalSubscriptionsAfter = new Dictionary<string, IGeneralRoutedEventSubscription>();
        }

        public void RegisterServiceDependency(Type dependantServiceType, Type masterServiceType)
        {
            dependencyGraph.AddDirectDependency(dependantServiceType, masterServiceType);
        }

        public void Subscribe<TEvent>(string subscriptionName, Action<TEvent> handlerAction, IReadOnlyList<Type> affectedServiceTypes)
            where TEvent : IRoutedEvent
        {
            ValidateEventType<TEvent>();
            var router = (IEventRouter<TEvent>)routers.GetOrAdd(typeof(TEvent), x => new EventRouter<TEvent>());
            router.Subscribe(subscriptionName, handlerAction, affectedServiceTypes);
        }

        public void SubscribeToAllBefore(string subscriptionName, Action<IRoutedEvent> handlerAction, bool handleStopped)
        {
            generalSubscriptionsBefore.Add(subscriptionName, new GeneralRoutedEventSubscription(subscriptionName, handlerAction, handleStopped));
        }

        public void SubscribeToAllAfter(string subscriptionName, Action<IRoutedEvent> handlerAction, bool handleStopped)
        {
            generalSubscriptionsAfter.Add(subscriptionName, new GeneralRoutedEventSubscription(subscriptionName, handlerAction, handleStopped));
        }

        public void SortSubscriptionsByDependencies(Action<EventSortingContradiction> onContradiction)
        {
            foreach (var router in routers.Values)
                if (!router.TrySortSubscriptionsByDependencies(dependencyGraph, out var contradictionString))
                    onContradiction(new EventSortingContradiction(router.EventType, contradictionString));
        }

        public IReadOnlyList<IEventRoutingCustomList> BuildCustomLists()
        {
            return routers.Values
                .Select(router => new EventRoutingCustomList(router.EventType, router.GetSubscriptionNames().ToArray()))
                .Cast<IEventRoutingCustomList>()
                .ToArray();
        }

        public void ApplyCustomLists(IReadOnlyList<IEventRoutingCustomList> lists, Action<string> onConflict)
        {
            foreach (var list in lists)
            {
                if (routers.TryGetValue(list.EventType, out var router))
                    router.ApplyCustomList(list, dependencyGraph, x => onConflict($"Custom list conflict for event '{router.EventType.FullName}': {x}"));
                else
                    onConflict($"Event '{list.EventType.FullName}' not found.");
            }
        }

        public void FireEvent<TEvent>(TEvent ev) where TEvent : IRoutedEvent
        {
            ValidateEventType<TEvent>();

            foreach (var subscribtion in generalSubscriptionsBefore.Values)
                if (!ev.StopPropagation || subscribtion.HandleStopped)
                    subscribtion.HandlerAction(ev);

            var router = (IEventRouter<TEvent>)routers.GetOrAdd(typeof(TEvent), x => new EventRouter<TEvent>());
            foreach (var subscribtion in router.Subscriptions)
            {
                if (ev.StopPropagation)
                    break;
                subscribtion.HandlerAction(ev);
            }

            foreach (var subscribtion in generalSubscriptionsAfter.Values)
                if (!ev.StopPropagation || subscribtion.HandleStopped)
                    subscribtion.HandlerAction(ev);
        }

        private static void ValidateEventType<TEvent>()
        {
            if (!typeof(TEvent).IsInterface)
                throw new ArgumentException("Event type must be an interface");
        }
    }
}