using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct Vector4 : IEquatable<Vector4>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        #region Constructors
        public Vector4(float x, float y, float z, float w)
        {
            X = x; Y = y; Z = z; W = w;
        }

        public Vector4(Vector2 xy, float z, float w)
        {
            X = xy.X; Y = xy.Y; Z = z; W = w;
        }

        public Vector4(Vector3 xyz, float w)
        {
            X = xyz.X; Y = xyz.Y; Z = xyz.Z; W = w;
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
                    case 3: return W;
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
                    case 3: W = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Vector3 Xyz
        {
            get { return new Vector3(X, Y, Z); }
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return
                X.GetHashCode() ^
                (Y.GetHashCode() << 1) ^
                (Z.GetHashCode() << 2) ^
                (W.GetHashCode() << 3);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}, {3}}}",
                X.ToString(CultureInfo.InvariantCulture),
                Y.ToString(CultureInfo.InvariantCulture),
                Z.ToString(CultureInfo.InvariantCulture),
                W.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(Vector4 s1, Vector4 s2)
        {
            return
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z &&
                s1.W == s2.W;
        }

        public static bool operator ==(Vector4 s1, Vector4 s2) { return Equals(s1, s2); }
        public static bool operator !=(Vector4 s1, Vector4 s2) { return !Equals(s1, s2); }
        public bool Equals(Vector4 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Vector4 && Equals((Vector4)obj); }
        #endregion

        #region Math
        public float Length() { return MathHelper.Sqrt(LengthSquared()); }
        public float LengthSquared() { return X * X + Y * Y + Z * Z + W * W; }

        public Vector4 Normalize()
        {
            float invLength = 1.0f / Length();
            return new Vector4(X * invLength, Y * invLength, Z * invLength, W * invLength);
        }

        public static Vector4 operator -(Vector4 v) { return v.Negate(); }
        public Vector4 Negate() { return new Vector4(-X, -Y, -Z, -W); }

        public static Vector4 operator +(Vector4 v1, Vector4 v2) { return Add(v1, v2); }
        public static Vector4 Add(Vector4 v1, Vector4 v2)
        {
            return new Vector4(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z,
                v1.W + v2.W);
        }

        public static Vector4 operator -(Vector4 left, Vector4 right) { return Subtract(left, right); }
        public static Vector4 Subtract(Vector4 left, Vector4 right)
        {
            return new Vector4(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W);
        }

        public static float Distance(Vector4 v1, Vector4 v2) { return (v1 - v2).Length(); }
        public static float DistanceSquared(Vector4 v1, Vector4 v2) { return (v1 - v2).LengthSquared(); }

        public static Vector4 operator *(float scale, Vector4 v) { return v.ScaleBy(scale); }
        public static Vector4 operator *(Vector4 v, float scale) { return v.ScaleBy(scale); }
        public static Vector4 operator /(Vector4 v, float scale) { return v.ScaleBy(1f / scale); }
        public Vector4 ScaleBy(float scale) { return new Vector4(X * scale, Y * scale, Z * scale, W * scale); }

        public static float Dot(Vector4 v1, Vector4 v2)
        {
            return 
                v1.X * v2.X + 
                v1.Y * v2.Y + 
                v1.Z * v2.Z + 
                v1.W * v2.W;
        }

        public static Vector4 Lerp(Vector4 left, Vector4 right, float amount)
        {
            return new Vector4(
                MathHelper.Lerp(left.X, right.X, amount),
                MathHelper.Lerp(left.Y, right.Y, amount),
                MathHelper.Lerp(left.Z, right.Z, amount),
                MathHelper.Lerp(left.W, right.W, amount));
        }

        public static Vector4 NLerp(Vector4 left, Vector4 right, float amount)
        {
            return Lerp(left, right, amount).Normalize();
        }

        public static Vector4 Min(Vector4 v1, Vector4 v2)
        {
            return new Vector4(
                Math.Min(v1.X, v2.X),
                Math.Min(v1.Y, v2.Y),
                Math.Min(v1.Z, v2.Z),
                Math.Min(v1.W, v2.W));
        }

        public static Vector4 Max(Vector4 v1, Vector4 v2)
        {
            return new Vector4(
                Math.Max(v1.X, v2.X),
                Math.Max(v1.Y, v2.Y),
                Math.Max(v1.Z, v2.Z),
                Math.Max(v1.W, v2.W));
        }
        #endregion

        #region Constant Vectors
        public static Vector4 Zero { get { return new Vector4(); } }
        public static Vector4 UnitX { get { return new Vector4(1f, 0f, 0f, 0f); } }
        public static Vector4 UnitY { get { return new Vector4(0f, 1f, 0f, 0f); } }
        public static Vector4 UnitZ { get { return new Vector4(0f, 0f, 1f, 0f); } }
        public static Vector4 UnitW { get { return new Vector4(0f, 0f, 0f, 1f); } }
        #endregion
    }
}
