using System.Collections.Generic;
using System.Linq;

namespace Clarity.Ext.Rendering.Ogl3
{
    public unsafe class StandardTransparentRenderQueue : IRenderQueue
    {
        private readonly List<RenderQueueItem> elements = new List<RenderQueueItem>();

        public void Clear() => elements.Clear();
        public IEnumerable<RenderQueueItem> GetInOrder() => elements.OrderBy(x => -*(float*)&x.ValData);
        public void Enqueue(RenderQueueItem item) => elements.Add(item);
    }
}