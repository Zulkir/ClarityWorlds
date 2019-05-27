using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;
using JetBrains.Annotations;

namespace Clarity.Engine.Visualization.Elements
{
    public interface IModelVisualElement : IVisualElement
    {
        StandardVisualGroup VisualGroup { get; }

        [NotNull] IModel3D Model { get; }
        int ModelPartIndex { get; }
        [NotNull] IMaterial Material { get; }
        [NotNull] IRenderState RenderState { get; }

        Transform Transform { get; }
        Vector3 NonUniformScale { get; }
        TransformSpace TransformSpace { get; }

        ModelVisualElementImmutabilityFlags GetImmutability();

        float DistanceToCameraSq(Transform elemTransform, ICamera camera);
    }
}