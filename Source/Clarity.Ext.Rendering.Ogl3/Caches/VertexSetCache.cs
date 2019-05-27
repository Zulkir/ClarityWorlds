using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.CodingUtilities.Unmanaged;
using Clarity.Common.GraphicalGeometry;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.Caching;
using ObjectGL.Api.Objects.VertexArrays;

namespace Clarity.Ext.Rendering.Ogl3.Caches
{
    public class VertexSetCache : AutoDisposableBase, ICache
    {
        private readonly IGraphicsInfra infra;
        private readonly IFlexibleModelVertexSet vertexSet;
        private IVertexArray glVertexArray;
        private bool dirty;

        public VertexSetCache(IGraphicsInfra infra, IFlexibleModelVertexSet vertexSet)
        {
            this.infra = infra;
            this.vertexSet = vertexSet;
            dirty = true;
        }

        protected override void Dispose(bool explicitly)
        {
            infra.MainThreadDisposer.Add(glVertexArray);
        }

        public void OnMasterEvent(object eventArgs)
        {
            dirty = true;
        }
        
        public IVertexArray GetGlVao()
        {
            if (!dirty)
                return glVertexArray;

            if (glVertexArray == null)
                glVertexArray = infra.GlContext.Create.VertexArray();

            glVertexArray.DisableVertexAttributesStartingFrom(0);

            var indexOfPosition = vertexSet.ElementInfos.IndexOf(x => x.CommonSemantic == CommonVertexSemantic.Position);
            var indexOfNormal = vertexSet.ElementInfos.IndexOf(x => x.CommonSemantic == CommonVertexSemantic.Normal);
            var indexOfTexCoord = vertexSet.ElementInfos.IndexOf(x => x.CommonSemantic == CommonVertexSemantic.TexCoord);

            if (indexOfPosition.HasValue)
                SetVertexAttrF(0, indexOfPosition.Value, VertexAttributeDimension.Three, VertexAttribPointerType.Float);
            if (indexOfNormal.HasValue)
                SetVertexAttrF(1, indexOfNormal.Value, VertexAttributeDimension.Three, VertexAttribPointerType.Float);
            if (indexOfTexCoord.HasValue)
                SetVertexAttrF(2, indexOfTexCoord.Value, VertexAttributeDimension.Two, VertexAttribPointerType.Float);

            if (vertexSet.IndicesInfo != null)
            {
                var arraySubrange = vertexSet.ArraySubranges[vertexSet.IndicesInfo.ArrayIndex];
                var rawDataRes = arraySubrange.RawDataResource;
                var arrayCache = rawDataRes.CacheContainer.GetOrAddCache(Tuples.Pair(infra, rawDataRes), x => new RawDataResCache(x.First, x.Second));
                var glBuffer = arrayCache.GetGlIndexBuffer(arraySubrange.StartOffset);
                glVertexArray.SetElementArrayBuffer(glBuffer);
            }
            else
            {
                glVertexArray.SetElementArrayBuffer(null);
            }

            dirty = false;
            return glVertexArray;
        }

        private void SetVertexAttrF(uint indexInVao, int indexInVertexSet, VertexAttributeDimension attrDimension, VertexAttribPointerType attrType)
        {
            var elemInfo = vertexSet.ElementInfos[indexInVertexSet];
            var arraySubrange = vertexSet.ArraySubranges[elemInfo.ArrayIndex];
            var rawDataRes = arraySubrange.RawDataResource;
            var arrayCache = rawDataRes.CacheContainer.GetOrAddCache(Tuples.Pair(infra, rawDataRes), x => new RawDataResCache(x.First, x.Second));
            var glBuffer = arrayCache.GetGlVertexBuffer();
            glVertexArray.SetVertexAttributeF(indexInVao, glBuffer, attrDimension, attrType, false, elemInfo.Stride, arraySubrange.StartOffset + elemInfo.Offset);
        }
    }
}