using System.Runtime.InteropServices;
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

        public Vector2? ZPlaneIntersection
        {
            get
            {
                var t = -Point.Z / Direction.Z;
                return t <= 0f ? (Vector2?)null : Point.Xy + Direction.Xy * t;
            }
        }

        public Line3 ToLine() => new Line3(Point, Direction);
    }
}