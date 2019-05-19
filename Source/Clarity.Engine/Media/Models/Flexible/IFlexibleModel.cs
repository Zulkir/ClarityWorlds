using System.Collections.Generic;
using Clarity.Engine.Resources;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Models.Flexible
{
    public interface IFlexibleModel : IResource
    {
        [NotNull, ItemNotNull]
        IReadOnlyList<IFlexibleModelVertexSet> VertexSets { get; }

        [NotNull, ItemNotNull]
        IReadOnlyList<IFlexibleModelPart> Parts { get; }

        // todo: default materials

        // todo: remove
        float Radius { get; }
    }
}