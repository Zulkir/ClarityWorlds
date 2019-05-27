namespace Clarity.Engine.EventRouting
{
    public class EventRoutingContext : IEventRoutingContext
    {
        public bool StopPropagation { get; set; }
    }
}