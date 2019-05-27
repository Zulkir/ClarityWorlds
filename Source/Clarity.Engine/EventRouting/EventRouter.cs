using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Engine.EventRouting
{
    public class EventRouter<TEvent> : IEventRouter<TEvent>
        where TEvent : IRoutedEvent
    {
        private enum SubscriptionDependencyComparisonResult
        {
            Unrelated,
            FirstDependsOnSecond,
            SecondDependsOnFirst,
            Contradicting
        }

        public Type EventType => typeof(TEvent);
        private readonly List<RoutedEventSubscription<TEvent>> subscriptions;

        public IReadOnlyList<IRoutedEventSubscription<TEvent>> Subscriptions => subscriptions;

        public EventRouter()
        {
            subscriptions = new List<RoutedEventSubscription<TEvent>>();
        }

        public IEnumerable<string> GetSubscriptionNames()
        {
            return Subscriptions.Select(x => x.Name);
        }

        public void Subscribe(string subscriptionName, Action<TEvent> handlerAction,
            IReadOnlyList<Type> affectedServiceTypes)
        {
            subscriptions.Add(new RoutedEventSubscription<TEvent>(subscriptionName, handlerAction, affectedServiceTypes));
        }

        public bool TrySortSubscriptionsByDependencies(IServiceEventDependencyGraph dependencyGraph, out string contradictionString)
        {
            while (true)
            {
                var foundWrongOrder = false;
                for (var i = 0; i < Subscriptions.Count - 1; i++)
                for (var j = i + 1; j < Subscriptions.Count; j++)
                {
                    var comparisonResult = CompareSubscriptions(Subscriptions[i], Subscriptions[j], dependencyGraph, out contradictionString);
                    if (comparisonResult == SubscriptionDependencyComparisonResult.Contradicting)
                        return false;
                    if (comparisonResult != SubscriptionDependencyComparisonResult.FirstDependsOnSecond)
                            continue;
                    CodingHelper.Swap(subscriptions, i, j);
                    foundWrongOrder = true;
                }
                if (!foundWrongOrder)
                    break;
            }
            contradictionString = null;
            return true;
        }

        public void ApplyCustomList(IEventRoutingCustomList list, IServiceEventDependencyGraph dependencyGraph, Action<string> onConflict)
        {
            var subscriptionsByName = subscriptions.ToDictionary(x => x.Name);
            subscriptions.Clear();
            foreach (var subName in list.SubscriptionNames)
            {
                if (subscriptionsByName.TryGetValue(subName, out var sub))
                    subscriptions.Add(sub);
                else
                    onConflict($"Subscription '{subName}' not found");
            }

            foreach (var subPair in subscriptions.AllPairs())
            {
                var comparisonResult = CompareSubscriptions(subPair.First, subPair.Second, dependencyGraph, out var dependencyString);
                if (comparisonResult == SubscriptionDependencyComparisonResult.Contradicting)
                    onConflict($"Subscriptions '{subPair.First.Name}' and '{subPair.Second.Name}' are contradicting:\n{dependencyString}");
                else if (comparisonResult == SubscriptionDependencyComparisonResult.FirstDependsOnSecond)
                    onConflict($"Subscription '{subPair.First.Name}' depends on '{subPair.Second.Name}', but comes first:\n{dependencyString}");
            }
        }

        private static SubscriptionDependencyComparisonResult CompareSubscriptions(IRoutedEventSubscription sub1, IRoutedEventSubscription sub2, 
                                                                                  IServiceEventDependencyGraph dependencyGraph, out string dependencyStrings)
        {
            var forwardDependencies = sub1.AffectedServiceTypes
                .SelectMany(x => sub2.AffectedServiceTypes.Select(y => Tuples.SameTypePair(x, y)))
                .Where(x => dependencyGraph.DependencyExists(x.First, x.Second))
                .ToArray();
            var backwardDependencies = sub1.AffectedServiceTypes
                .SelectMany(x => sub2.AffectedServiceTypes.Select(y => Tuples.SameTypePair(x, y)))
                .Where(x => dependencyGraph.DependencyExists(x.Second, x.First))
                .ToArray();

            var forwardLines = forwardDependencies.Select(x => $"    {sub1.Name} ({x.First.Name}) -> ({x.Second.Name}) {sub2.Name}");
            var backwardLines = forwardDependencies.Select(x => $"    {sub1.Name} ({x.First.Name}) <- ({x.Second.Name}) {sub2.Name}");
            dependencyStrings = string.Join("\n", forwardLines.Concat(backwardLines));

            if (forwardDependencies.Length == 0 && backwardDependencies.Length == 0)
                return SubscriptionDependencyComparisonResult.Unrelated;
            if (forwardDependencies.Length > 0 && backwardDependencies.Length == 0)
                return SubscriptionDependencyComparisonResult.FirstDependsOnSecond;
            if (forwardDependencies.Length == 0 && backwardDependencies.Length > 0)
                return SubscriptionDependencyComparisonResult.SecondDependsOnFirst;
            return SubscriptionDependencyComparisonResult.Contradicting;
        }
    }
}