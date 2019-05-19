using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray2
    {
        public Vector2 Point;
        public Vector2 Direction;

        public Ray2(Vector2 point, Vector2 direction)
        {
            Point = point;
            Direction = direction.Normalize();
        }

        public Line2 ToLine() => new Line2(Point, Direction);
    }
}