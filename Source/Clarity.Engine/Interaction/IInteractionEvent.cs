using Clarity.Engine.EventRouting;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Interaction
{
    public interface IInteractionEvent : IRoutedEvent
    {
        IViewport Viewport { get; }
    }
}