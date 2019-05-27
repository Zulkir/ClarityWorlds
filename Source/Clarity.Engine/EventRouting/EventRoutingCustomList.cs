using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public class EventRoutingCustomList : IEventRoutingCustomList
    {
        public string EventName { get; }
        public IReadOnlyList<string> SubscriptionNames { get; }

        public EventRoutingCustomList(string eventName, IReadOnlyList<string> subscriptionNames)
        {
            EventName = eventName;
            SubscriptionNames = subscriptionNames;
        }
    }
}