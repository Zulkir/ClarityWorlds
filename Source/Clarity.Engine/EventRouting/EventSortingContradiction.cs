using System;

namespace Clarity.Engine.EventRouting
{
    public struct EventSortingContradiction
    {
        public Type EventType;
        public string ContradictionString;

        public EventSortingContradiction(Type eventType, string contradictionString)
        {
            EventType = eventType;
            ContradictionString = contradictionString;
        }
    }
}