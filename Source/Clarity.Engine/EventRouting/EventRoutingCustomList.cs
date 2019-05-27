using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public class EventRoutingCustomList : IEventRoutingCustomList
    {
        public Type EventType { get; }
        public IReadOnlyList<string> SubscriptionNames { get; }

        public EventRoutingCustomList(Type eventType, IReadOnlyList<string> subscriptionNames)
        {
            EventType = eventType;
            SubscriptionNames = subscriptionNames;
        }
    }
}