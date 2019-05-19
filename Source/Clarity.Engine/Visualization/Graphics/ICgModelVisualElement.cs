using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Visualization.Graphics.Materials;
using JetBrains.Annotations;

namespace Clarity.Engine.Visualization.Graphics
{
    public interface ICgModelVisualElement : ICgVisualElement
    {
        [NotNull] IFlexibleModel Model { get; }
        int ModelPartIndex { get; }
        [NotNull] IMaterial Material { get; }
        Transform Transform { get; }
        Vector3 NonUniformScale { get; }
        CgTransformSpace TransformSpace { get; }
        CgCullFace CullFace { get; }
        CgPolygonMode PolygonMode { get; }
        float ZOffset { get; }
        CgHighlightEffect HighlightEffect { get; }

        CgModelVisualElementImmutabilityFlags GetImmutability();
    }
}