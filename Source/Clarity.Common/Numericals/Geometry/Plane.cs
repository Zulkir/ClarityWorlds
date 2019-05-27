using System;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Plane : IEquatable<Plane>
    {
        public Vector3 Normal;
        public float D;

        public Plane(Vector3 roughNormal, float d)
        {
            if (d < 0)
            {
                Normal = roughNormal.Normalize();
                D = d;
            }
            else if (d > 0)
            {
                Normal = -roughNormal.Normalize();
                D = -d;
            }
            else
            {
                Normal = roughNormal.ToPositiveDirection().Normalize();
                D = 0;
            }
        }

        public Plane(Vector3 roughNormal, Vector3 point)
        {
            Normal = roughNormal.ToPositiveDirection().Normalize();
            D = -Vector3.Dot(Normal, point);
        }

        #region Equality
        public bool Equals(Plane other) => 
            Normal.Equals(other.Normal) && 
            D.Equals(other.D);

        public override bool Equals(object obj) => 
            obj is Plane && Equals((Plane)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Normal.GetHashCode() * 397) ^ D.GetHashCode();
            }
        }
        #endregion

        public Vector3 Project(Vector3 point)
        {
            return point - Normal * (Vector3.Dot(point, Normal) + D);
        }

        public static void Intersect(Plane p1, Plane p2, out Line3? lineResult)
        {
            var n1 = p1.Normal;
            var n2 = p2.Normal;
            var d1 = p1.D;
            var d2 = p2.D;
            var u = Vector3.Cross(n1, n2);
            var denom = u.LengthSquared();
            if (denom == 0)
            {
                lineResult = null;
                return;
            }
            var point = Vector3.Cross((d2 * n1 + d1 * n2), u) / denom;
            var dir = u;
            lineResult = new Line3(point, dir);
        }
    }
}