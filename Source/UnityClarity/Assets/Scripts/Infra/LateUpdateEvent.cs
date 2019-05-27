using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;

namespace Assets.Scripts.Infra
{
    public class LateUpdateEvent : RoutedEventBase, ILateUpdateEvent
    {
        public FrameTime FrameTime { get; }

        public LateUpdateEvent(FrameTime frameTime)
        {
            FrameTime = frameTime;
        }
    }
}