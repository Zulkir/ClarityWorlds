using Clarity.Engine.EventRouting;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Interaction
{
    public interface IInteractionEventArgs : IRoutedEvent
    {
        IViewport Viewport { get; }
    }
}