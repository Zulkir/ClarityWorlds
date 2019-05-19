using System.Collections.Generic;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class StandardOpaqueRenderQueue : IRenderQueue
    {
        private readonly List<RenderQueueItem> elements = new List<RenderQueueItem>();

        public void Clear() => elements.Clear();
        public void Enqueue(RenderQueueItem item) => elements.Add(item);
        public IEnumerable<RenderQueueItem> GetInOrder() => elements;
    }
}