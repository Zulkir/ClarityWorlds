namespace Clarity.Engine.EventRouting
{
    public interface IEventRoutingContext
    {
        bool StopPropagation { get; set; }
    }
}