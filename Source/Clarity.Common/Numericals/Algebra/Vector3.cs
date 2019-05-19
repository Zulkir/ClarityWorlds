using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X;
        public float Y;
        public float Z;

        #region Constructors
        public Vector3(float value)
        {
            X = Y = Z = value;
        }

        public Vector3(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        public Vector3(Vector2 xy, float z)
        {
            X = xy.X; Y = xy.Y; Z = z;
        }
        #endregion

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Vector2 Xy
        {
            get { return new Vector2(X, Y); }
            set { X = value.X; Y = value.Y; }
        }

        public Vector2 Xz
        {
            get { return new Vector2(X, Z); }
            set { X = value.X; Z = value.Y; }
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return
                X.GetHashCode() ^
                (Y.GetHashCode() << 1) ^
                (Z.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}}}",
                X.ToString(CultureInfo.InvariantCulture),
                Y.ToString(CultureInfo.InvariantCulture),
                Z.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(Vector3 s1, Vector3 s2)
        {
            return
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z;
        }

        public static bool operator ==(Vector3 s1, Vector3 s2) { return Equals(s1, s2); }
        public static bool operator !=(Vector3 s1, Vector3 s2) { return !Equals(s1, s2); }
        public bool Equals(Vector3 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Vector3 && Equals((Vector3)obj); }
        #endregion

        #region Math
        public float Length() { return MathHelper.Sqrt(LengthSquared()); }
        public float LengthSquared() { return X * X + Y * Y + Z * Z; }

        public Vector3 Normalize()
        {
            float invLength = 1.0f / Length();
            return new Vector3(X * invLength, Y * invLength, Z * invLength);
        }

        public Vector3 ToPositiveDirection()
        {
            var firstNonZero = X != 0f ? X : Y != 0f ? Y : Z;
            return firstNonZero == 0f
                ? UnitZ :
                (firstNonZero > 0 ? this : -this);
        }

        public Vector3 Abs() => 
            new Vector3(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

        public static Vector3 operator -(Vector3 v) { return v.Negate(); }
        public Vector3 Negate() { return new Vector3(-X, -Y, -Z);}

        public static Vector3 operator +(Vector3 v1, Vector3 v2) { return Add(v1, v2); }
        public static Vector3 Add(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 left, Vector3 right) { return Subtract(left, right); }
        public static Vector3 Subtract(Vector3 left, Vector3 right)
        {
            return new Vector3(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z);
        }

        public static float Distance(Vector3 v1, Vector3 v2) { return (v1 - v2).Length(); }
        public static float DistanceSquared(Vector3 v1, Vector3 v2) { return (v1 - v2).LengthSquared(); }

        public static Vector3 operator *(float scale, Vector3 v) { return v.ScaleBy(scale); }
        public static Vector3 operator *(Vector3 v, float scale) { return v.ScaleBy(scale); }
        public static Vector3 operator /(Vector3 v, float scale) { return v.ScaleBy(1f / scale); }
        public Vector3 ScaleBy(float scale) { return new Vector3(X * scale, Y * scale, Z * scale); }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return 
                v1.X * v2.X + 
                v1.Y * v2.Y + 
                v1.Z * v2.Z;
        }

        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            return new Vector3(
                left.Y * right.Z - left.Z * right.Y,
                left.Z * right.X - left.X * right.Z,
                left.X * right.Y - left.Y * right.X);
        }

        public static Vector3 Lerp(Vector3 left, Vector3 right, float amount)
        {
            return new Vector3(
                MathHelper.Lerp(left.X, right.X, amount),
                MathHelper.Lerp(left.Y, right.Y, amount),
                MathHelper.Lerp(left.Z, right.Z, amount));
        }

        public static Vector3 NLerp(Vector3 left, Vector3 right, float amount)
        {
            return Lerp(left, right, amount).Normalize();
        }

        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                Math.Min(v1.X, v2.X),
                Math.Min(v1.Y, v2.Y),
                Math.Min(v1.Z, v2.Z));
        }

        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                Math.Max(v1.X, v2.X),
                Math.Max(v1.Y, v2.Y),
                Math.Max(v1.Z, v2.Z));
        }
        #endregion

        #region Constant Vectors
        public static Vector3 Zero { get { return new Vector3(); } }
        public static Vector3 UnitX { get { return new Vector3(1f, 0f, 0f); } }
        public static Vector3 UnitY { get { return new Vector3(0f, 1f, 0f); } }
        public static Vector3 UnitZ { get { return new Vector3(0f, 0f, 1f); } }
        #endregion
    }
}
