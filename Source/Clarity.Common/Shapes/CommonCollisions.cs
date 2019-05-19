using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Common.Shapes
{
    public static class CommonCollisions
    {
        public static Vector3? CollisionPoint(Ray3 ray, Sphere sphere)
        {
            var toCenter = sphere.Center - ray.Point;
            var distance = Vector3.Dot(toCenter, ray.Direction);
            var closestRayPoint = ray.Point + ray.Direction * distance;
            if ((closestRayPoint - sphere.Center).LengthSquared() > sphere.Radius * sphere.Radius)
                return null;
            // todo: calculate correct hit point
            return closestRayPoint;
        }
    }
}