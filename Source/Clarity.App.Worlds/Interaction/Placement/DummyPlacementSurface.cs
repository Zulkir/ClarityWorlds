using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.App.Worlds.Interaction.Placement 
{
    public class DummyPlacementSurface : IPlacementSurface
    {
        public bool TryFindPlace(Ray3 globalRay, out Transform localTransform) 
        { 
            localTransform = default(Transform);
            return false;
        }

        public bool TryFindPoint2D(Ray3 globalRay, out Vector2 point2D)
        {
            point2D = default(Vector2);
            return false;
        }

        public Transform Point2DToPlace(Vector2 point2D) => Transform.Translation(point2D.X, point2D.Y, 0);
        public Vector2 PlaceToPoint2D(Transform localTransform) => localTransform.Offset.Xy;
    }
}