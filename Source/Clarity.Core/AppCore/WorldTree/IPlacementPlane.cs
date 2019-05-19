using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface IPlacementPlane
    {
        //Transform GetRelativeTransform(ISceneNode child);
        bool TryFindPoint2D(Ray3 globalRay, out Vector2 point);
        bool TryFindPlace(Ray3 globalRay, out Transform localTransform);
    }
}