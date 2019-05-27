using Clarity.Engine.EventRouting;

namespace Clarity.Engine.Platforms
{
    public class NewFrameEvent : RoutedEventBase, INewFrameEvent
    {
        public FrameTime FrameTime { get; }

        public NewFrameEvent(FrameTime frameTime)
        {
            FrameTime = frameTime;
        }
    }
}