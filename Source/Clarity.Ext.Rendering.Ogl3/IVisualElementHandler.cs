using Clarity.Engine.Visualization.Components;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IVisualElementHandler
    {
        bool CanHandle(IVisualElement element);
        void OnTraverse(ISceneRenderingContext context, IVisualElement visualElement);
        void OnDequeue(ISceneRenderingContext context, RenderQueueItem queueItem);
    }
}