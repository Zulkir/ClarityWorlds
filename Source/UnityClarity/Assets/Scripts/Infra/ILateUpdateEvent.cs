using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;

namespace Assets.Scripts.Infra
{
    public interface ILateUpdateEvent : IRoutedEvent
    {
        FrameTime FrameTime { get; }
    }
}