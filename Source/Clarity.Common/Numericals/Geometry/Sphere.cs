using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct Sphere : IEquatable<Sphere>
    {
        public Vector3 Center;
        public float Radius;

        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        #region Equality, Hash, String
        public override int GetHashCode() => 
            Center.GetHashCode() ^ (Radius.GetHashCode() << 1);

        public override string ToString() => 
            $"{{{Center}, {Radius.ToString(CultureInfo.InvariantCulture)}}}";

        public static bool Equals(Sphere s1, Sphere s2) => 
            s1.Center == s2.Center && s1.Radius == s2.Radius;

        public static bool operator ==(Sphere s1, Sphere s2) => Equals(s1, s2);
        public static bool operator !=(Sphere s1, Sphere s2) => !Equals(s1, s2);
        public bool Equals(Sphere other) => Equals(this, other);
        public override bool Equals(object obj) => obj is Sphere && Equals((Sphere)obj);
        #endregion

        public bool Contains(Vector3 point, float radiusBias = 0)
        {
            var biasedRadius = Radius + radiusBias;
            return (point - Center).LengthSquared() <= biasedRadius * biasedRadius;
        }

        public Vector3 Project(Vector3 point)
        {
            var rayDir = point - Center;
            var length = rayDir.Length();
            return length > MathHelper.Eps5 
                ? Center + rayDir / length * Radius 
                : Center + Vector3.UnitZ * Radius;
        }

        public void Intersect(LineSegment3 segment, out Vector3? p1, out Vector3? p2)
        {
            var dpp = segment.Point2 - segment.Point1;
            var dpc = segment.Point1 - Center;
            var a = dpp.LengthSquared();
            var b = Vector3.Dot(dpp, dpc);
            var c = dpc.LengthSquared() - Radius * Radius;
            var discr = b * b - a * c;
            if (discr < 0)
            {
                p1 = p2 = null;
            }
            else if (discr < MathHelper.Eps5)
            {
                var l = -b / a;
                p1 = 0 <= l && l <= 1 
                    ? (Vector3?)Vector3.Lerp(segment.Point1, segment.Point2, l) 
                    : null;
                p2 = null;
            }
            else
            {
                var sqrdscr = MathHelper.Sqrt(discr);
                var l1 = (-b + sqrdscr) / a;
                var l2 = (-b - sqrdscr) / a;
                p1 = 0 <= l1 && l1 <= 1 ? (Vector3?)Vector3.Lerp(segment.Point1, segment.Point2, l1) : null;
                p2 = 0 <= l2 && l2 <= 1 ? (Vector3?)Vector3.Lerp(segment.Point1, segment.Point2, l2) : null;
            }
        }

        public void Intersect(Sphere sphere, out Sphere? sphereResult, out Circle3? circleResult, out Vector3? pointResult) =>
            Intersect(this, sphere, out sphereResult, out circleResult, out pointResult);

        public static void Intersect(Sphere s1, Sphere s2, out Sphere? sphereResult, out Circle3? circleResult, out Vector3? pointResult)
        {
            sphereResult = null;
            circleResult = null;
            pointResult = null;
            if (s1.Center == s2.Center)
            {
                if (s1.Radius == s2.Radius)
                    sphereResult = s1;
                return;
            }
            if (s1.Radius < s2.Radius)
                CodingHelper.Swap(ref s1, ref s2);
            var firstToSecond = s2.Center - s1.Center;
            var distance = firstToSecond.Length();
            var dir = firstToSecond / distance;
            var radiusSum = s1.Radius + s2.Radius;
            if (radiusSum < distance)
                return;
            if (radiusSum == distance)
            {
                pointResult = s1.Center + dir * s1.Radius;
                return;
            }
            if (distance + s2.Radius > s1.Radius)
            {
                var offset = (distance.Sq() + s1.Radius.Sq() - s2.Radius.Sq()) / (2 * distance);
                var center = s1.Center + dir * offset;
                var radius = MathHelper.Sqrt(s1.Radius * s1.Radius - offset * offset);
                circleResult = new Circle3(center, dir, radius);
                return;
            }
            if (distance + s2.Radius == s1.Radius)
            {
                pointResult = s1.Center + dir * s1.Radius;
                return;
            }
        }

        public static Sphere BoundingSphere(AaBox box)
        {
            return new Sphere(box.Center, box.HalfSize.ToVector().Length());
        }

        // todo: improve precision
        public static Sphere BoundingSphere(IReadOnlyList<Sphere> spheres)
        {
            var boundingBox = AaBox.BoundingBox(spheres);

            const float normalizedStep = 0.01f;
            var scale = Math.Max(Math.Max(boundingBox.HalfSize.Width, boundingBox.HalfSize.Height), boundingBox.HalfSize.Depth);
            var step = scale * normalizedStep;

            var center = boundingBox.Center;
            for (int i = 0; i < 0.25f / normalizedStep; i++)
            {
                var mostDistantSphere = spheres.Maximal(x => (center - x.Center).Length() + x.Radius);
                var towards = mostDistantSphere.Center - center;
                var towardsLength = towards.Length();
                if (towardsLength > MathHelper.Eps5)
                    center += step * towards / towardsLength;
            }

            var finalMostDistantSphere = spheres.Maximal(x => (center - x.Center).Length() + x.Radius);
            return new Sphere(center, (center - finalMostDistantSphere.Center).Length() + finalMostDistantSphere.Radius);
        }

        public static Sphere BoundingSphere(IReadOnlyList<Vector3> points)
        {
            var boundingBox = AaBox.BoundingBox(points);

            const float normalizedStep = 0.01f;
            var scale = Math.Max(Math.Max(boundingBox.HalfSize.Width, boundingBox.HalfSize.Height), boundingBox.HalfSize.Depth);
            var step = scale * normalizedStep;

            var center = boundingBox.Center;
            for (int i = 0; i < 0.25f / normalizedStep; i++)
            {
                var mostDistantPoint = points.Maximal(x => (center - x).LengthSquared());
                var towards = mostDistantPoint - center;
                var towardsLength = towards.Length();
                if (towardsLength > MathHelper.Eps5)
                    center += step * towards / towardsLength;
            }

            var finalMostDistantPoint = points.Maximal(x => (center - x).LengthSquared());
            return new Sphere(center, (center - finalMostDistantPoint).Length());
        }

        public static Sphere BoundingSphere<T>(IReadOnlyList<T> vertices, Func<T, Vector3> getPosition)
        {
            var boundingBox = AaBox.BoundingBox(vertices.Select(getPosition));

            const float normalizedStep = 0.01f;
            var scale = Math.Max(Math.Max(boundingBox.HalfSize.Width, boundingBox.HalfSize.Height), boundingBox.HalfSize.Depth);
            var step = scale * normalizedStep;

            var center = boundingBox.Center;
            for (int i = 0; i < 0.25f / normalizedStep; i++)
            {
                var mostDistantPoint = vertices.Select(getPosition).Maximal(x => (center - x).LengthSquared());
                var towards = mostDistantPoint - center;
                var towardsLength = towards.Length();
                if (towardsLength > MathHelper.Eps5)
                    center += step * towards / towardsLength;
            }

            var finalMostDistantPoint = vertices.Select(getPosition).Maximal(x => (center - x).LengthSquared());
            return new Sphere(center, (center - finalMostDistantPoint).Length());
        }

        public static Sphere operator *(Sphere s, Transform tr) { return Apply(s, tr); }
        public static Sphere Apply(Sphere s, Transform tr)
        {
            return new Sphere(s.Center * tr, s.Radius * tr.Scale);
        }
    }
}