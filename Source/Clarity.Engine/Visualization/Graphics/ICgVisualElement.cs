using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Components;

namespace Clarity.Engine.Visualization.Graphics
{
    public interface ICgVisualElement : IVisualElement
    {
        CgBasicVisualGroup BasicVisualGroup { get; }
        ICgVisualElementExtension NextExtension { get; }
        float DistanceToCameraSq(Transform elemTransform, ICamera camera);
    }
}