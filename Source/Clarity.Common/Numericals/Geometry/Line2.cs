using System;
using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Line2 : IEquatable<Line2>
    {
        public Vector2 Point;
        public Vector2 Direction;

        public Line2(Vector2 point, Vector2 roughDirection)
        {
            Direction = NormalizeDirection(roughDirection);
            Point = NormalizePoint(point, Direction);
        }

        private static Vector2 NormalizeDirection(Vector2 dir)
        {
            var firstNonZero = dir.X != 0f ? dir.X : dir.Y;
            return firstNonZero == 0f
                ? Vector2.UnitX
                : (firstNonZero > 0 ? dir : -dir).Normalize();
        }

        private static Vector2 NormalizePoint(Vector2 point, Vector2 direction)
        {
            var pd = Vector2.Dot(point, direction);
            var dd = Vector2.Dot(direction, direction);
            var t = -pd / dd;
            return point + direction * t;
        }

        public bool Equals(Line2 other) =>
            Point.Equals(other.Point) && Direction.Equals(other.Direction);

        public override bool Equals(object obj) =>
            obj is Line2 && Equals((Line2)obj);

        public override int GetHashCode() =>
            (Point.GetHashCode() * 397) ^ Direction.GetHashCode();

        public Vector2? Intersect(Line2 line)
        {
            GetClosestPoints(this, line, out var p1, out var p2);
            return (p1 - p2).LengthSquared() < MathHelper.Eps5 ? p1 : (Vector2?)null;
        }

        public static void GetClosestPoints(Line2 l1, Line2 l2, out Vector2 cp1, out Vector2 cp2)
        {
            var p1 = l1.Point;
            var p2 = l2.Point;
            var d1 = l1.Direction;
            var d2 = l2.Direction;
            var d1d2 = Vector2.Dot(d1, d2);
            var b = d1d2;
            var pdiff = p2 - p1;
            var denom = 1 - b.Sq();
            float t1, t2;
            if (denom < MathHelper.Eps5)
            {
                t2 = 0;
                t1 = Vector2.Dot(d1, pdiff);
            }
            else
            {
                t1 = Vector2.Dot(pdiff, (d1 - b * d2)) / denom;
                t2 = d1d2 * t1 - Vector2.Dot(d2, pdiff);
            }
            cp1 = p1 + d1 * t1;
            cp2 = p2 + d2 * t2;
        }
    }
}