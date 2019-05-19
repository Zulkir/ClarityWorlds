using System.Collections.Generic;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IRenderQueue
    {
        void Clear();
        void Enqueue(RenderQueueItem item);
        IEnumerable<RenderQueueItem> GetInOrder();
    }
}