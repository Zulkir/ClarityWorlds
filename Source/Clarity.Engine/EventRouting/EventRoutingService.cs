using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Engine.EventRouting
{
    public class EventRoutingService : IEventRoutingService
    {
        private readonly IServiceEventDependencyGraph dependencyGraph;
        private readonly Dictionary<string, IRoutedEvent> events;

        public EventRoutingService()
        {
            dependencyGraph = new ServiceEventDependencyGraph();
            events = new Dictionary<string, IRoutedEvent>();
        }

        public void RegisterServiceDependency(Type dependantServiceType, Type masterServiceType)
        {
            dependencyGraph.AddDirectDependency(dependantServiceType, masterServiceType);
        }

        public void Subscribe<TArgs>(string eventName, string subscriptionName, 
            Action<IEventRoutingContext, TArgs> handlerAction, IReadOnlyList<Type> affectedServiceTypes)
        {
            var evnt = (IRoutedEvent<TArgs>)events.GetOrAdd(eventName, x => new RoutedEvent<TArgs>(eventName));
            evnt.Subscribe(subscriptionName, handlerAction, affectedServiceTypes);
        }

        public void SortSubscriptionsByDependencies(Action<EventSortingContradiction> onContradiction)
        {
            foreach (var evnt in events.Values)
                if (!evnt.TrySortSubscriptionsByDependencies(dependencyGraph, out var contradictionString))
                    onContradiction(new EventSortingContradiction(evnt.Name, contradictionString));
        }

        public IReadOnlyList<IEventRoutingCustomList> BuildCustomLists()
        {
            return events.Values
                .Select(evnt => new EventRoutingCustomList(evnt.Name, evnt.GetSubscriptionNames().ToArray()))
                .Cast<IEventRoutingCustomList>()
                .ToArray();
        }

        public void ApplyCustomLists(IReadOnlyList<IEventRoutingCustomList> lists, Action<string> onConflict)
        {
            foreach (var list in lists)
            {
                if (events.TryGetValue(list.EventName, out var evnt))
                    evnt.ApplyCustomList(list, dependencyGraph, x => onConflict($"Custom list conflict for event '{evnt.Name}': {x}"));
                else
                    onConflict($"Event '{list.EventName}' not found.");
            }
        }

        public void FireEvent<TArgs>(string eventName, TArgs args)
        {
            var evnt = (IRoutedEvent<TArgs>)events.GetOrAdd(eventName, x => new RoutedEvent<TArgs>(eventName));
            var context = new EventRoutingContext();
            foreach (var subscribtion in evnt.Subscriptions)
            {
                subscribtion.HandlerAction(context, args);
                if (context.StopPropagation)
                    break;
            }
        }
    }
}