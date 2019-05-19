using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 8)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X;
        public float Y;

        #region Constructors
        public Vector2(float value)
        {
            X = Y = value;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
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
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = 0; break;
                    case 1: Y = 0; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return
                X.GetHashCode() ^
                (Y.GetHashCode() << 1);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}",
                X.ToString(CultureInfo.InvariantCulture),
                Y.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(Vector2 s1, Vector2 s2)
        {
            return
                s1.X == s2.X &&
                s1.Y == s2.Y;
        }

        public static bool operator ==(Vector2 s1, Vector2 s2) { return Equals(s1, s2); }
        public static bool operator !=(Vector2 s1, Vector2 s2) { return !Equals(s1, s2); }
        public bool Equals(Vector2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Vector2 && Equals((Vector2)obj); }
        #endregion

        #region Math
        public float Length() { return MathHelper.Sqrt(LengthSquared()); }
        public float LengthSquared() { return X * X + Y * Y; }

        public Vector2 Normalize()
        {
            float invLength = 1.0f / Length();
            return new Vector2(X * invLength, Y * invLength);
        }

        public static Vector2 operator -(Vector2 v) { return v.Negate(); }
        public Vector2 Negate() { return new Vector2(-X, -Y); }

        public static Vector2 operator +(Vector2 v1, Vector2 v2) { return Add(v1, v2); }
        public static Vector2 Add(Vector2 v1, Vector2 v2)
        {
            return new Vector2(
                v1.X + v2.X, 
                v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 left, Vector2 right) { return Subtract(left, right); }
        public static Vector2 Subtract(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.X - right.X,
                left.Y - right.Y);
        }

        public static float Distance(Vector2 v1, Vector2 v2) { return (v1 - v2).Length(); }
        public static float DistanceSquared(Vector2 v1, Vector2 v2) { return (v1 - v2).LengthSquared(); }

        public static Vector2 operator *(float scale, Vector2 v) { return v.ScaleBy(scale); }
        public static Vector2 operator *(Vector2 v, float scale) { return v.ScaleBy(scale); }
        public static Vector2 operator /(Vector2 v, float scale) { return v.ScaleBy(1f / scale); }
        public Vector2 ScaleBy(float scale) { return new Vector2(X * scale, Y * scale); }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return 
                v1.X * v2.X +
                v1.Y * v2.Y;
        }

        public static float Cross(Vector2 left, Vector2 right)
        {
            return 
                left.X * right.Y -
                left.Y * right.X;
        }

        public static Vector2 Lerp(Vector2 left, Vector2 right, float amount)
        {
            return new Vector2(
                MathHelper.Lerp(left.X, right.X, amount),
                MathHelper.Lerp(left.Y, right.Y, amount));
        }

        public static Vector2 NLerp(Vector2 left, Vector2 right, float amount)
        {
            return Lerp(left, right, amount).Normalize();
        }

        public static Vector2 Min(Vector2 v1, Vector2 v2)
        {
            return new Vector2(
                Math.Min(v1.X, v2.X),
                Math.Min(v1.Y, v2.Y));
        }

        public static Vector2 Max(Vector2 v1, Vector2 v2)
        {
            return new Vector2(
                Math.Max(v1.X, v2.X),
                Math.Max(v1.Y, v2.Y));
        }
        #endregion

        #region Constant Vectors
        public static Vector2 Zero { get { return new Vector2(); } }
        public static Vector2 UnitX { get { return new Vector2(1f, 0f); } }
        public static Vector2 UnitY { get { return new Vector2(0f, 1f); } }
        #endregion
    }
}
