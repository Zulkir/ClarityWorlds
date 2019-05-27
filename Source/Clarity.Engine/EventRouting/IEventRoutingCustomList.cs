using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public interface IEventRoutingCustomList
    {
        string EventName { get; }
        IReadOnlyList<string> SubscriptionNames { get; }
    }
}