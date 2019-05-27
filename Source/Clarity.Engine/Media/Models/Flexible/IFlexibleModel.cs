using System.Collections.Generic;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Models.Flexible
{
    public interface IFlexibleModel : IModel3D
    {
        [NotNull, ItemNotNull]
        IReadOnlyList<IFlexibleModelVertexSet> VertexSets { get; }

        [NotNull, ItemNotNull]
        IReadOnlyList<IFlexibleModelPart> Parts { get; }

        // todo: default materials
    }
}