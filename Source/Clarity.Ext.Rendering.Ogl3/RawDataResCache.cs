using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Unmanaged;
using Clarity.Engine.Objects.Caching;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;
using ObjectGL.Api.Objects.Resources.Buffers;
using PtrMagic;

namespace Clarity.Ext.Rendering.Ogl3
{
    public unsafe class RawDataResCache : AutoDisposableBase, ICache
    {
        private readonly IGraphicsInfra infra;
        private readonly IRawDataResource rawDataResource;
        private IBuffer glVertexBuffer;
        private IBuffer glIndexBuffer;
        private Dictionary<int, IBuffer> glIndexBuffersWithOffsets;
        private bool dirty;

        public RawDataResCache(IGraphicsInfra infra, IRawDataResource rawDataResource)
        {
            this.infra = infra;
            this.rawDataResource = rawDataResource;
            dirty = true;
        }

        protected override void Dispose(bool explicitly)
        {
            infra.MainThreadDisposer.Add(glVertexBuffer);
            infra.MainThreadDisposer.Add(glIndexBuffer);
        }

        public void OnMasterEvent(object eventArgs)
        {
            if (glVertexBuffer != null)
                GetGlVertexBuffer();
            if (glIndexBuffer != null)
                GetGlIndexBuffer(0);
            dirty = true;
        }

        public IBuffer GetGlVertexBuffer() => 
            GetGlBuffer(ref glVertexBuffer, 0);

        public IBuffer GetGlIndexBuffer(int offset)
        {
            if (offset == 0)
                return GetGlBuffer(ref glIndexBuffer, 0);
            if (glIndexBuffersWithOffsets == null)
                glIndexBuffersWithOffsets = new Dictionary<int, IBuffer>();
            var old = glIndexBuffersWithOffsets.TryGetRef(offset);
            var modifyed = old;
            modifyed = GetGlBuffer(ref modifyed, offset);
            if (modifyed != old)
                glIndexBuffersWithOffsets[offset] = modifyed;
            return modifyed;
        }

        private IBuffer GetGlBuffer(ref IBuffer glBuffer, int offset)
        {
            if (!dirty)
                return glBuffer;
            var nonDynamic = rawDataResource.Volatility != ResourceVolatility.Volatile;
            var resourcePtr = rawDataResource.Map();
            var resourceOffsetPtr = resourcePtr + offset;
            var glBufferSize = rawDataResource.Size - offset;
            if (glBuffer == null || glBuffer.SizeInBytes != glBufferSize)
            {
                glBuffer?.Dispose();
                var usage = nonDynamic
                    ? BufferUsageHint.StaticDraw
                    : BufferUsageHint.DynamicDraw;
                glBuffer = infra.GlContext.Create.Buffer(BufferTarget.Array, glBufferSize, usage, resourceOffsetPtr);
            }
            else if (nonDynamic)
            {
                glBuffer.SetData(resourceOffsetPtr);
            }
            else
            {
                var bufferPtr = glBuffer.Map(0, glBufferSize, MapAccess.Write | MapAccess.InvalidateBuffer);
                PtrHelper.CopyBulk((byte*)bufferPtr, (byte*)resourceOffsetPtr, glBufferSize);
                glBuffer.Unmap();
            }
            rawDataResource.Unmap(false);
            dirty = false;
            return glBuffer;
        }
    }
}