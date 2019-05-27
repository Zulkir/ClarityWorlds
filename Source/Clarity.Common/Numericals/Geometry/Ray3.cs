using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray3
    {
        public Vector3 Point;
        public Vector3 Direction;

        public Ray3(Vector3 point, Vector3 direction)
        {
            Point = point;
            Direction = direction.Normalize();
        }

        public Vector2? IntersectZPlane()
        {
            if (Direction.Z.Abs() < MathHelper.Eps5)
                return null;
            var t = -Point.Z / Direction.Z;
            return t >= 0 ? Point.Xy + Direction.Xy * t : (Vector2?)null;
        }

        public Line3 ToLine() => new Line3(Point, Direction);

        public Vector3? Intersect(Plane plane)
        {
            var nd = Vector3.Dot(plane.Normal, Direction);
            if (nd.Abs() < MathHelper.Eps5)
                return null;
            var np = Vector3.Dot(plane.Normal, Point);
            var t = -(np + plane.D) / nd;
            return t >= 0 ? Point + Direction * t : (Vector3?)null;
        }
    }
}