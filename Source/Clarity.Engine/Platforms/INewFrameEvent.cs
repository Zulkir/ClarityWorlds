using Clarity.Engine.EventRouting;

namespace Clarity.Engine.Platforms
{
    public interface INewFrameEvent : IRoutedEvent
    {
        FrameTime FrameTime { get; }
    }
}