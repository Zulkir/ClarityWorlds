using System;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Line3 : IEquatable<Line3>
    {
        public Vector3 Point;
        public Vector3 Direction;

        public Line3(Vector3 point, Vector3 roughDirection)
        {
            Direction = NormalizeDirection(roughDirection);
            Point = NormalizePoint(point, Direction);
        }

        private static Vector3 NormalizeDirection(Vector3 dir)
        {
            var firstNonZero = dir.X != 0f ? dir.X : dir.Y != 0f ? dir.Y : dir.Z;
            return firstNonZero == 0f
                ? Vector3.UnitZ :
                (firstNonZero > 0 ? dir : -dir).Normalize();
        }

        private static Vector3 NormalizePoint(Vector3 point, Vector3 direction)
        {
            var pd = Vector3.Dot(point, direction);
            var dd = Vector3.Dot(direction, direction);
            var t = -pd / dd;
            return point + direction * t;
        }

        public bool Equals(Line3 other) => 
            Point.Equals(other.Point) && Direction.Equals(other.Direction);
        public override bool Equals(object obj) => 
            obj is Line3 && Equals((Line3)obj);
        public override int GetHashCode() => 
            (Point.GetHashCode() * 397) ^ Direction.GetHashCode();

        public static void GetClosestPoints(Line3 l1, Line3 l2, out Vector3 cp1, out Vector3 cp2)
        {
            var p1 = l1.Point;
            var p2 = l2.Point;
            var d1 = l1.Direction;
            var d2 = l2.Direction;
            var d1d2 = Vector3.Dot(d1, d2);
            var pdiff = p1 - p2;
            if (1 - Math.Abs(d1d2) < MathHelper.Eps5)
            {
                var t1 = 0;
                var t2 = Vector3.Dot(d1, pdiff) / Vector3.Dot(d1, d2);
                cp1 = p1 + d1 * t1;
                cp2 = p2 + d2 * t2;
            }
            else
            {
                var b = d1d2;
                var b2 = b * b;
                var dnm = 1f / (1 - b2);
                var c1 = -Vector3.Dot(d1, pdiff);
                var c2 = -Vector3.Dot(d2, pdiff);
                var t1 = dnm * (b * c2 - c1);
                var t2 = dnm * (c2 - b * c1);
                cp1 = p1 + d1 * t1;
                cp2 = p2 + d2 * t2;
            }
        }
    }
}