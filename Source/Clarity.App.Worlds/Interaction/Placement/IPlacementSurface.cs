using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.App.Worlds.Interaction.Placement
{
    public interface IPlacementSurface
    {
        bool TryFindPlace(Ray3 globalRay, out Transform localTransform);
        bool TryFindPoint2D(Ray3 globalRay, out Vector2 point2D);
        Transform Point2DToPlace(Vector2 point2D);
        Vector2 PlaceToPoint2D(Transform localTransform);
    }
}