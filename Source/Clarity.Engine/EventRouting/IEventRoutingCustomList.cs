using System;
using System.Collections.Generic;

namespace Clarity.Engine.EventRouting
{
    public interface IEventRoutingCustomList
    {
        Type EventType { get; }
        IReadOnlyList<string> SubscriptionNames { get; }
    }
}