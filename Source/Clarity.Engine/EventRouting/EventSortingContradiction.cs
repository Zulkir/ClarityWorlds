namespace Clarity.Engine.EventRouting
{
    public struct EventSortingContradiction
    {
        public string EventName;
        public string ContradictionString;

        public EventSortingContradiction(string eventName, string contradictionString)
        {
            EventName = eventName;
            ContradictionString = contradictionString;
        }
    }
}