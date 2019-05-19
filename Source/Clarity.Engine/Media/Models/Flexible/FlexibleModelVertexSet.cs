using System.Collections.Generic;
using Clarity.Common.GraphicalGeometry;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;

namespace Clarity.Engine.Media.Models.Flexible
{
    public class FlexibleModelVertexSet : ResourceBase, IFlexibleModelVertexSet
    {
        public IReadOnlyList<RawDataResSubrange> ArraySubranges { get; }
        public IReadOnlyList<IVertexElementInfo> ElementInfos { get; }
        public IVertexIndicesInfo IndicesInfo { get; }

        public FlexibleModelVertexSet(ResourceVolatility volatility, 
            IReadOnlyList<RawDataResSubrange> arraySubranges, 
            IReadOnlyList<IVertexElementInfo> elementInfos, 
            IVertexIndicesInfo indicesInfo) 
            : base(volatility)
        {
            ArraySubranges = arraySubranges;
            ElementInfos = elementInfos;
            IndicesInfo = indicesInfo;
        }
    }
}