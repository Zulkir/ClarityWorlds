using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Elements;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining
{
    public struct RenderQueueItem
    {
        public IVisualElement VisualElement;
        public ISceneNode Node;
        public bool IsHighlighted;

        public RenderQueueItem(IVisualElement visualElement, IVisualElementHandler handler, ISceneNode node, bool isHighlighted)
        {
            VisualElement = visualElement;
            Node = node;
            IsHighlighted = isHighlighted;
        }
    }
}