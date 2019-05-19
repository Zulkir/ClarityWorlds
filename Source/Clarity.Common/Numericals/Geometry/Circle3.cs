using System;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Circle3 : IEquatable<Circle3>
    {
        public Vector3 Center;
        public Vector3 Normal;
        public float Radius;

        public Circle3(Vector3 center, Vector3 roughNormal, float radius)
        {
            this.Center = center;
            this.Radius = radius;
            Normal = roughNormal.ToPositiveDirection().Normalize();
        }

        public bool Equals(Circle3 other) => 
            Center.Equals(other.Center) && 
            Normal.Equals(other.Normal) && 
            Radius.Equals(other.Radius);

        public override bool Equals(object obj) => 
            obj is Circle3 && Equals((Circle3)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Center.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.GetHashCode();
                hashCode = (hashCode * 397) ^ Radius.GetHashCode();
                return hashCode;
            }
        }
        
        public Plane GetPlane() => new Plane(Normal, Center);

        public Vector3 Project(Vector3 point)
        {
            var projPoint = GetPlane().Project(point);
            var rayDir = projPoint - Center;
            var length = rayDir.Length();
            if (length > MathHelper.Eps5)
                return Center + rayDir / length * Radius;
            var notNormal = Math.Abs(Normal.X) < MathHelper.Eps5
                ? new Vector3(1, Normal.Y, Normal.Z)
                : new Vector3(0, Normal.Y, Normal.Z);
            var newDir = Vector3.Cross(Normal, notNormal).Normalize();
            return Center + newDir * Radius;
        }

        public static void Intersect(Circle3 circle, Sphere sphere, out Circle3? circleResult, out Vector3? pointResult1, out Vector3? pointResult2)
        {
            circleResult = null;
            pointResult1 = null;
            pointResult2 = null;

            sphere.Intersect(new Sphere(circle.Center, circle.Radius), out var globSphereResult, out var globCircleResult, out var globPointResult);
            if (globSphereResult.HasValue)
            {
                circleResult = circle;
                return;
            }
            if (globCircleResult.HasValue)
            {
                var cr = globCircleResult.Value;
                var lineDir = Vector3.Cross(circle.Normal, cr.Normal);
                pointResult1 = cr.Center + lineDir * cr.Radius;
                pointResult2 = cr.Center - lineDir * cr.Radius;
                return;
            }
            if (globPointResult.HasValue)
            {
                pointResult1 = globPointResult.Value;
            }
        }
    }
}