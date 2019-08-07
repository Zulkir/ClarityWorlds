using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 8)]
    public struct IntVector2 : IEquatable<IntVector2>
    {
        public int X, Y;

        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public IntVector2(Vector2 floatVector)
        {
            X = (int)floatVector.X;
            Y = (int)floatVector.Y;
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return X ^ (Y << 16);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}",
                X.ToString(CultureInfo.InvariantCulture),
                Y.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(IntVector2 s1, IntVector2 s2)
        {
            return 
                s1.X == s2.X &&
                s1.Y == s2.Y;
        }

        public static bool operator ==(IntVector2 s1, IntVector2 s2) { return Equals(s1, s2); }
        public static bool operator !=(IntVector2 s1, IntVector2 s2) { return !Equals(s1, s2); }
        public bool Equals(IntVector2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is IntVector2 && Equals((IntVector2)obj); }
        #endregion

        #region Math
        public float Length() { return MathHelper.Sqrt(LengthSquared()); }
        public int LengthSquared() { return X * X + Y * Y; }

        public static IntVector2 operator -(IntVector2 v) { return v.Negate(); }
        public IntVector2 Negate() { return new IntVector2(-X, -Y); }

        public static IntVector2 operator +(IntVector2 v1, IntVector2 v2) { return Add(v1, v2); }
        public static IntVector2 Add(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(
                v1.X + v2.X,
                v1.Y + v2.Y);
        }

        public static IntVector2 operator -(IntVector2 left, IntVector2 right) { return Subtract(left, right); }
        public static IntVector2 Subtract(IntVector2 left, IntVector2 right)
        {
            return new IntVector2(
                left.X - right.X,
                left.Y - right.Y);
        }

        public static float Distance(IntVector2 v1, IntVector2 v2) { return (v1 - v2).Length(); }
        public static int DistanceSquared(IntVector2 v1, IntVector2 v2) { return (v1 - v2).LengthSquared(); }

        public static IntVector2 operator *(int scale, IntVector2 v) { return v.ScaleBy(scale); }
        public static IntVector2 operator *(IntVector2 v, int scale) { return v.ScaleBy(scale); }
        public IntVector2 ScaleBy(int scale) { return new IntVector2(X * scale, Y * scale); }

        public static int Dot(IntVector2 v1, IntVector2 v2)
        {
            return
                v1.X * v2.X +
                v1.Y * v2.Y;
        }
        #endregion

        #region Cast
        public static explicit operator Vector2(IntVector2 iv) => new Vector2(iv.X, iv.Y);
        public Vector2 ToVector() => new Vector2(X, Y);
        #endregion

        #region Constant Vectors
        public static IntVector2 Zero => new IntVector2();
        public static IntVector2 UnitX => new IntVector2(1, 0);
        public static IntVector2 UnitY => new IntVector2(0, 1);
        #endregion
    }
}
