using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 32)]
    public struct Transform : IEquatable<Transform>
    {
        public Quaternion Rotation;
        public Vector3 Offset;
        public float Scale;

        public Transform(float scale, Quaternion rotation, Vector3 offset)
        {
            Scale = scale;
            Rotation = rotation;
            Offset = offset;
        }

        public Transform(float s, float qx, float qy, float qz, float qw, float tx, float ty, float tz)
        {
            Scale = s;
            Rotation = new Quaternion(qx, qy, qz, qw);
            Offset = new Vector3(tx, ty, tz);
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return 
                Rotation.GetHashCode() ^
                (Offset.GetHashCode() << 1) ^
                (Scale.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return string.Format("{{S: {0}; R: {1}; T: {2}}}",
                Scale.ToString(CultureInfo.InvariantCulture),
                Rotation.ToString(),
                Offset.ToString());
        }

        public static bool Equals(Transform s1, Transform s2)
        {
            return 
                s1.Rotation == s2.Rotation &&
                s1.Offset == s2.Offset &&
                s1.Scale == s2.Scale;
        }

        public static bool operator ==(Transform s1, Transform s2) { return Equals(s1, s2); }
        public static bool operator !=(Transform s1, Transform s2) { return !Equals(s1, s2); }
        public bool Equals(Transform other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Transform && Equals((Transform)obj); }
        #endregion

        #region Math
        public Matrix4x4 ToMatrix4x4()
        {
            var result = Scale * Rotation.ToMatrix4x4();
            result.R3 = new Vector4(Offset, 1f);
            return result;
        }

        public Transform Invert()
        {
            var invScale = 1f / Scale;
            var invRotation = Rotation.Conjugate();
            var newOffset = -invScale * (Offset * invRotation);
            return new Transform(invScale, invRotation, newOffset);
        }

        public static Vector3 operator *(Vector3 v, Transform tr) { return Apply(v, tr); }
        public static Vector3 Apply(Vector3 v, Transform tr)
        {
            return tr.Scale * (v * tr.Rotation) + tr.Offset;
        }

        public static Ray3 operator *(Ray3 r, Transform tr) { return Apply(r, tr); }
        public static Ray3 Apply(Ray3 r, Transform tr)
        {
            var p1 = r.Point * tr;
            var p2 = (r.Point + r.Direction) * tr;
            return new Ray3(p1, p2 - p1);
            //return new Ray3(r.Point * tr, r.Direction * tr.Rotation);
        }

        public static Sphere operator *(Sphere s, Transform tr) { return Apply(s, tr); }
        public static Sphere Apply(Sphere s, Transform tr)
        {
            return new Sphere(s.Center * tr, s.Radius * tr.Scale);
        }

        public static Transform operator *(Transform left, Transform right) { return Combine(left, right); }
        public static Transform Combine(Transform left, Transform right)
        {
            return new Transform(
                left.Scale * right.Scale,
                left.Rotation * right.Rotation,
                left.Offset * right);
        }

        public static Transform DisjointCombine(Transform left, Transform right)
        {
            return new Transform(
                left.Scale * right.Scale,
                left.Rotation * right.Rotation,
                left.Offset + right.Offset);
        }

        public static Transform Lerp(Transform left, Transform right, float amount)
        {
            return new Transform(
                MathHelper.Lerp(left.Scale, right.Scale, amount),
                Quaternion.NLerp(left.Rotation, right.Rotation, amount),
                Vector3.Lerp(left.Offset, right.Offset, amount));
        }
        #endregion

        #region Constant Transforms
        public static Transform Identity { get { return new Transform(1f, Quaternion.Identity, Vector3.Zero); } }
        #endregion

        #region Typical transforms
        public static Transform Scaling(float scale)
        {
            return new Transform { Scale = scale, Rotation = Quaternion.Identity, Offset = Vector3.Zero };
        }

        public static Transform Rotate(Quaternion rotation)
        {
            return new Transform { Scale = 1, Rotation = rotation, Offset = Vector3.Zero };
        }

        public static Transform Translation(Vector3 translation)
        {
            return new Transform { Scale = 1, Rotation = Quaternion.Identity, Offset = translation };
        }

        public static Transform Translation(float x, float y, float z)
        {
            return new Transform { Scale = 1, Rotation = Quaternion.Identity, Offset = new Vector3(x, y, z) };
        }
        #endregion
    }
}