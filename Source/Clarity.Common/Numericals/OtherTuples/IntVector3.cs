using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    public struct IntVector3 : IEquatable<IntVector3>
    {
        public int X, Y, Z;

        public IntVector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return X ^ (Y << 10) ^ (Z << 20);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}}}",
                X.ToString(CultureInfo.InvariantCulture),
                Y.ToString(CultureInfo.InvariantCulture),
                Z.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(IntVector3 s1, IntVector3 s2)
        {
            return 
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z;
        }

        public static bool operator ==(IntVector3 s1, IntVector3 s2) { return Equals(s1, s2); }
        public static bool operator !=(IntVector3 s1, IntVector3 s2) { return !Equals(s1, s2); }
        public bool Equals(IntVector3 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is IntVector3 && Equals((IntVector3)obj); }
        #endregion

        #region Math
        public float Length() { return MathHelper.Sqrt(LengthSquared()); }
        public int LengthSquared() { return X * X + Y * Y + Z * Z; }

        public static IntVector3 operator -(IntVector3 v) { return v.Negate(); }
        public IntVector3 Negate() { return new IntVector3(-X, -Y, -Z); }

        public static IntVector3 operator +(IntVector3 v1, IntVector3 v2) { return Add(v1, v2); }
        public static IntVector3 Add(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z);
        }

        public static IntVector3 operator -(IntVector3 left, IntVector3 right) { return Subtract(left, right); }
        public static IntVector3 Subtract(IntVector3 left, IntVector3 right)
        {
            return new IntVector3(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z);
        }

        public static float Distance(IntVector3 v1, IntVector3 v2) { return (v1 - v2).Length(); }
        public static int DistanceSquared(IntVector3 v1, IntVector3 v2) { return (v1 - v2).LengthSquared(); }

        public static IntVector3 operator *(int scale, IntVector3 v) { return v.ScaleBy(scale); }
        public static IntVector3 operator *(IntVector3 v, int scale) { return v.ScaleBy(scale); }
        public IntVector3 ScaleBy(int scale) { return new IntVector3(X * scale, Y * scale, Z * scale); }

        public static int Dot(IntVector3 v1, IntVector3 v2)
        {
            return
                v1.X * v2.X +
                v1.Y * v2.Y +
                v1.Z * v2.Z;
        }
        #endregion
    }
}
