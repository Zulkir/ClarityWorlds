using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Interaction
{
    public interface IInteractionEventArgs
    {
        IViewport Viewport { get; }
    }
}