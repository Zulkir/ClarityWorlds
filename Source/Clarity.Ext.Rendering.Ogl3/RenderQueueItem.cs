using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Components;

namespace Clarity.Ext.Rendering.Ogl3
{
    public struct RenderQueueItem
    {
        public IVisualElement VisualElement;
        public IVisualElementHandler Handler;
        public ISceneNode Node;
        public object RefData;
        public long ValData;
    }
}