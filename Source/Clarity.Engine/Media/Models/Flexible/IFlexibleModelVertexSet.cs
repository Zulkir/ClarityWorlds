using System.Collections.Generic;
using Clarity.Common.GraphicalGeometry;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Models.Flexible
{
    public interface IFlexibleModelVertexSet : IResource
    {
        [NotNull]
        IReadOnlyList<RawDataResSubrange> ArraySubranges { get; }

        [NotNull, ItemNotNull]
        IReadOnlyList<IVertexElementInfo> ElementInfos { get; }

        [CanBeNull]
        IVertexIndicesInfo IndicesInfo { get; }
    }
}