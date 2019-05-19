using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public abstract class PlacementPlaneComponent : SceneNodeComponentBase<PlacementPlaneComponent>, IPlacementPlaneComponent, IPlacementPlane
    {
        public abstract bool Is2D { get; set; }

        public IPlacementPlane PlacementPlane => this;

        public bool TryFindPoint2D(Ray3 globalRay, out Vector2 point)
        {
            var futureTransform = Node.GlobalTransform;
            var localRay = globalRay * futureTransform.Invert();
            var t = -localRay.Point.Z / localRay.Direction.Z;
            if (t <= 0f)
            {
                point = default(Vector2);
                return false;
            }
            point = localRay.Point.Xy + localRay.Direction.Xy * t;
            return true;
        }

        public bool TryFindPlace(Ray3 globalRay, out Transform localTransform)
        {
            var futureTransform = Node.GlobalTransform;
            var localRay = globalRay * futureTransform.Invert();
            var t = -localRay.Point.Z / localRay.Direction.Z;
            if (t <= 0f)
            {
                localTransform = default(Transform);
                return false;
            }
            localTransform = Transform.Translation(localRay.Point + localRay.Direction * t);
            return true;
        }
    }
}