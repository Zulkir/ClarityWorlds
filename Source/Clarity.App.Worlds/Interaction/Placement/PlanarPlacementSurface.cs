using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction.Placement
{
    public class PlanarPlacementSurface : IPlacementSurface
    {
        private readonly ISceneNodeBound master;
        private readonly Transform planeTransform;
        private readonly float childScaling;
        private readonly Quaternion childRotation;

        public PlanarPlacementSurface(ISceneNodeBound master, Transform planeTransform) 
            : this(master, planeTransform, planeTransform.Scale, planeTransform.Rotation) {}

        public PlanarPlacementSurface(ISceneNodeBound master, Transform planeTransform, float childScaling, Quaternion childRotation)
        {
            this.master = master;
            this.planeTransform = planeTransform;
            this.childScaling = childScaling;
            this.childRotation = childRotation;
        }

        public bool TryFindPoint2D(Ray3 globalRay, out Vector2 point2D)
        {
            var planeLocalRay = globalRay * (planeTransform * master.Node.GlobalTransform).Invert();
            var intersection = planeLocalRay.IntersectZPlane();
            if (!intersection.HasValue)
            {
                point2D = default(Vector2);
                return false;
            }
            point2D = intersection.Value;
            return true;
        }

        public bool TryFindPlace(Ray3 globalRay, out Transform localTransform)
        {
            if (!TryFindPoint2D(globalRay, out var point2D))
            {
                localTransform = default(Transform);
                return false;
            }
            localTransform = Point2DToPlace(point2D);
            return true;
        }

        public Transform Point2DToPlace(Vector2 point2D)
        {
            return new Transform(childScaling, childRotation, new Vector3(point2D, 0) * planeTransform);
        }

        public Vector2 PlaceToPoint2D(Transform localTransform)
        {
            var plane = new Plane(Vector3.UnitZ * planeTransform.Rotation, planeTransform.Offset);
            var point3D = plane.Project(localTransform.Offset);
            return (point3D * planeTransform.Invert()).Xy;
        }
    }
}