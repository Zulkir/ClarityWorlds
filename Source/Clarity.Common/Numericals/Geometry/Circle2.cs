using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Circle2 : IEquatable<Circle2>
    {
        public Vector2 Center;
        public float Radius;

        public Circle2(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public float Area => MathHelper.Pi * Radius.Sq();

        #region Equality, Hash, String
        public override int GetHashCode() => 
            Center.GetHashCode() ^ (Radius.GetHashCode() << 1);

        public override string ToString() => 
            $"{{{Center}, {Radius.ToString(CultureInfo.InvariantCulture)}}}";

        public static bool Equals(Circle2 s1, Circle2 s2) => 
            s1.Center == s2.Center && s1.Radius == s2.Radius;

        public static bool operator ==(Circle2 s1, Circle2 s2) => Equals(s1, s2);
        public static bool operator !=(Circle2 s1, Circle2 s2) => !Equals(s1, s2);
        public bool Equals(Circle2 other) => Equals(this, other);
        public override bool Equals(object obj) => obj is Circle2 && Equals((Circle2)obj);
        #endregion

        public bool Contains(Vector2 point, float radiusBias = 0)
        {
            var biasedRadius = Radius + radiusBias;
            return (point - Center).LengthSquared() <= biasedRadius * biasedRadius;
        }

        public bool Contains(Circle2 circle, float radiusBias = 0)
        {
            var biasedRadius = Radius + radiusBias;
            return (circle.Center - Center).Length() + circle.Radius <= biasedRadius;
        }

        public Vector2 Project(Vector2 point)
        {
            var rayDir = point - Center;
            var length = rayDir.Length();
            return length > MathHelper.Eps5 
                ? Center + rayDir / length * Radius 
                : Center + Vector2.UnitY * Radius;
        }

        public AaRectangle2 GetBoundingRect()
        {
            return new AaRectangle2(Center, Radius, Radius);
        }

        public IEnumerable<Vector2> Intersect(LineSegment2 segment)
        {
            var dpp = segment.Point2 - segment.Point1;
            var dpc = segment.Point1 - Center;
            var a = dpp.LengthSquared();
            var b = Vector2.Dot(dpp, dpc);
            var c = dpc.LengthSquared() - Radius * Radius;
            var discr = b * b - a * c;
            if (discr < 0)
                yield break;
            if (discr < MathHelper.Eps5)
            {
                var l = -b / a;
                if (0 <= l && l <= 1)
                    yield return Vector2.Lerp(segment.Point1, segment.Point2, l);
            }
            else
            {
                var sqrdscr = MathHelper.Sqrt(discr);
                var l1 = (-b + sqrdscr) / a;
                var l2 = (-b - sqrdscr) / a;
                if (0 <= l1 && l1 <= 1)
                    yield return Vector2.Lerp(segment.Point1, segment.Point2, l1);
                if (0 <= l2 && l2 <= 1)
                    yield return Vector2.Lerp(segment.Point1, segment.Point2, l2);
            }
        }

        public void Intersect(Circle2 circle2, out Circle2? circle2Result, out Vector2? pointRes1, out Vector2? pointRes2) =>
            Intersect(this, circle2, out circle2Result, out pointRes1, out pointRes2);

        public static void Intersect(Circle2 c1, Circle2 c2, out Circle2? circle2Result, out Vector2? pointRes1, out Vector2? pointRes2)
        {
            circle2Result = null;
            pointRes1 = null;
            pointRes2 = null;
            if (c1.Center == c2.Center)
            {
                if (c1.Radius == c2.Radius)
                    circle2Result = c1;
                return;
            }
            if (c1.Radius < c2.Radius)
                CodingHelper.Swap(ref c1, ref c2);
            var firstToSecond = c2.Center - c1.Center;
            var distance = firstToSecond.Length();
            var dir = firstToSecond / distance;
            var radiusSum = c1.Radius + c2.Radius;
            if (radiusSum < distance)
                return;
            if (radiusSum == distance)
            {
                pointRes1 = c1.Center + dir * c1.Radius;
                return;
            }
            if (distance + c2.Radius > c1.Radius)
            {
                var offset = (distance.Sq() + c1.Radius.Sq() - c2.Radius.Sq()) / (2 * distance);
                var center = c1.Center + dir * offset;
                var radius = MathHelper.Sqrt(c1.Radius.Sq() - offset.Sq());
                var ortho = new Vector2(dir.Y, -dir.X).Normalize();
                pointRes1 = center + ortho * radius;
                pointRes2 = center - ortho * radius;
                return;
            }
            if (distance + c2.Radius == c1.Radius)
            {
                pointRes1 = c1.Center + dir * c1.Radius;
                return;
            }
        }

        public static Circle2 BoundingCircle(IReadOnlyList<Circle2> innerCircles)
        {
            var result = innerCircles.Count >= 1 ? innerCircles[0] : new Circle2();
            
            for (int i = 0; i < innerCircles.Count; i++)
            for (int j = i + 1; j < innerCircles.Count; j++)
            {
                var pairCircle = BoundingCircle(innerCircles[i], innerCircles[j]);
                if (pairCircle.Radius > result.Radius)
                    result = pairCircle;
            }

            for (int i = 0; i < innerCircles.Count; i++)
            for (int j = i + 1; j < innerCircles.Count; j++)
            for (int k = j + 1; k < innerCircles.Count; k++)
            {
                // todo: remove check?
                var pairIJ = BoundingCircle(innerCircles[i], innerCircles[j]);
                var pairIK = BoundingCircle(innerCircles[i], innerCircles[k]);
                var pairJI = BoundingCircle(innerCircles[j], innerCircles[k]);
                if (pairIJ.Contains(innerCircles[k]) || pairIK.Contains(innerCircles[j]) || pairJI.Contains(innerCircles[i]))
                    continue;
                
                var pairCircle = CircumscribedCircle(innerCircles[i], innerCircles[j], innerCircles[k]);
                if (pairCircle.Radius > result.Radius)
                    result = pairCircle;
            }

            return result;
        }

        private static Circle2 BoundingCircle(Circle2 c1, Circle2 c2)
        {
            var offset = c2.Center - c1.Center;
            var d = offset.Length();
            if (d < MathHelper.Eps8)
                return c1;
            var radius = (c1.Radius + c2.Radius + d) / 2;
            var center = c1.Center + offset / d * (radius - c1.Radius);
            return new Circle2(center, radius);
        }

        private static Circle2 CircumscribedCircle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var d12 = p1 - p2;
            var d23 = p2 - p3;
            var d31 = p3 - p1;
            var d21 = -d12;
            var d32 = -d23;
            var d13 = -d31;

            var len12 = d12.Length();
            var len23 = d23.Length();
            var len31 = d31.Length();

            var alpha = (len23.Sq() * Vector2.Dot(d12, d13)) / (2 * Vector2.Cross(d12, d23).Sq());
            var beta  = (len31.Sq() * Vector2.Dot(d21, d23)) / (2 * Vector2.Cross(d12, d23).Sq());
            var gamma = (len12.Sq() * Vector2.Dot(d31, d32)) / (2 * Vector2.Cross(d12, d23).Sq());

            var center = alpha * p1 + beta * p2 + gamma * p3;
            var radius = (center - p1).Length();
            return new Circle2(center, radius);
        }

        private static Circle2 CircumscribedCircle(Circle2 circle1, Circle2 circle2, Circle2 circle3)
        {
            // todo: make it readable
            float 
                x1 = circle1.Center.X, y1 = circle1.Center.Y, r1 = circle1.Radius,
                x2 = circle2.Center.X, y2 = circle2.Center.Y, r2 = circle2.Radius,
                x3 = circle3.Center.X, y3 = circle3.Center.Y, r3 = circle3.Radius,
                a2 = 2 * (x1 - x2),
                b2 = 2 * (y1 - y2),
                c2 = 2 * (r2 - r1),
                d2 = x1 * x1 + y1 * y1 - r1 * r1 - x2 * x2 - y2 * y2 + r2 * r2,
                a3 = 2 * (x1 - x3),
                b3 = 2 * (y1 - y3),
                c3 = 2 * (r3 - r1),
                d3 = x1 * x1 + y1 * y1 - r1 * r1 - x3 * x3 - y3 * y3 + r3 * r3,
                ab = a3 * b2 - a2 * b3,
                xa = (b2 * d3 - b3 * d2) / ab - x1,
                xb = (b3 * c2 - b2 * c3) / ab,
                ya = (a3 * d2 - a2 * d3) / ab - y1,
                yb = (a2 * c3 - a3 * c2) / ab,
                a = xb * xb + yb * yb - 1,
                b = 2 * (xa * xb + ya * yb + r1),
                c = xa * xa + ya * ya - r1 * r1,
                r = (-b - MathHelper.Sqrt(b * b - 4 * a * c)) / (2 * a);
            return new Circle2(new Vector2(xa + xb * r + x1, ya + yb * r + y1), r);
        }
    }
}