using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct IntVector4 : IEquatable<IntVector4>
    {
        public int X, Y, Z, W;

        public IntVector4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return X ^ (Y << 8) ^ (Z << 16) ^ (W << 24);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}, {3}}}",
                X.ToString(),
                Y.ToString(),
                Z.ToString(),
                W.ToString());
        }

        public static bool Equals(IntVector4 s1, IntVector4 s2)
        {
            return 
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z &&
                s1.W == s2.W;
        }

        public static bool operator ==(IntVector4 s1, IntVector4 s2) { return Equals(s1, s2); }
        public static bool operator !=(IntVector4 s1, IntVector4 s2) { return !Equals(s1, s2); }
        public bool Equals(IntVector4 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is IntVector4 && Equals((IntVector4)obj); }
        #endregion

        #region Math
        public float Length() { return MathHelper.Sqrt(LengthSquared()); }
        public int LengthSquared() { return X * X + Y * Y + Z * Z + W * W; }

        public static IntVector4 operator -(IntVector4 v) { return v.Negate(); }
        public IntVector4 Negate() { return new IntVector4(-X, -Y, -Z, -W); }

        public static IntVector4 operator +(IntVector4 v1, IntVector4 v2) { return Add(v1, v2); }
        public static IntVector4 Add(IntVector4 v1, IntVector4 v2)
        {
            return new IntVector4(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z,
                v1.W + v2.W);
        }

        public static IntVector4 operator -(IntVector4 left, IntVector4 right) { return Subtract(left, right); }
        public static IntVector4 Subtract(IntVector4 left, IntVector4 right)
        {
            return new IntVector4(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W);
        }

        public static float Distance(IntVector4 v1, IntVector4 v2) { return (v1 - v2).Length(); }
        public static int DistanceSquared(IntVector4 v1, IntVector4 v2) { return (v1 - v2).LengthSquared(); }

        public static IntVector4 operator *(int scale, IntVector4 v) { return v.ScaleBy(scale); }
        public static IntVector4 operator *(IntVector4 v, int scale) { return v.ScaleBy(scale); }
        public IntVector4 ScaleBy(int scale) { return new IntVector4(X * scale, Y * scale, Z * scale, W * scale); }

        public static int Dot(IntVector4 v1, IntVector4 v2)
        {
            return
                v1.X * v2.X +
                v1.Y * v2.Y +
                v1.Z * v2.Z +
                v1.W * v2.W;
        }
        #endregion
    }
}
