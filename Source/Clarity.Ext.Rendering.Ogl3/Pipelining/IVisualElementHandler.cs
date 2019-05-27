using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Elements;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining
{
    public interface IVisualElementHandler
    {
        bool CanHandle(IVisualElement element);

        bool HasTransparency(RenderQueueItem queueItem);
        float GetCameraDistSq(RenderQueueItem queueItem, ICamera camera);

        void Draw(RenderQueueItem queueItem, ICamera camera, float aspectRatio);
    }
}