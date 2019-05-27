using System;
using System.Globalization;

namespace Clarity.Common.Numericals.Algebra
{
    public struct DVector2 : IEquatable<DVector2>
    {
        public double X;
        public double Y;

        #region Constructors
        public DVector2(double value)
        {
            X = Y = value;
        }

        public DVector2(double x, double y)
        {
            X = x;
            Y = y;
        }
        #endregion

        public double this[int index]
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
        public override int GetHashCode() => 
            X.GetHashCode() ^
            (Y.GetHashCode() << 1);

        public override string ToString() => string.Format("{{{0}, {1}}}",
            X.ToString(CultureInfo.InvariantCulture),
            Y.ToString(CultureInfo.InvariantCulture));

        public static bool Equals(in DVector2 s1, in DVector2 s2) => 
            s1.X == s2.X &&
            s1.Y == s2.Y;

        public static bool operator ==(in DVector2 s1, in DVector2 s2) => Equals(s1, s2);
        public static bool operator !=(in DVector2 s1, in DVector2 s2) => !Equals(s1, s2);
        public bool Equals(DVector2 other) => Equals(this, other);
        public override bool Equals(object obj) => obj is DVector2 && Equals((DVector2)obj);
        #endregion

        #region Math
        public double Length() => Math.Sqrt(LengthSquared());
        public double LengthSquared() => X * X + Y * Y;

        public DVector2 Normalize()
        {
            var invLength = 1.0 / Length();
            return new DVector2(X * invLength, Y * invLength);
        }

        public static DVector2 operator -(in DVector2 v) => v.Negate();
        public DVector2 Negate() => new DVector2(-X, -Y);

        public static DVector2 operator +(in DVector2 v1, in DVector2 v2) => Add(v1, v2);
        public static DVector2 Add(in DVector2 v1, in DVector2 v2) => new DVector2(
            v1.X + v2.X, 
            v1.Y + v2.Y);

        public static DVector2 operator -(in DVector2 left, in DVector2 right) => Subtract(left, right);
        public static DVector2 Subtract(in DVector2 left, in DVector2 right) => new DVector2(
            left.X - right.X,
            left.Y - right.Y);

        public static double Distance(in DVector2 v1, in DVector2 v2) => (v1 - v2).Length();
        public static double DistanceSquared(in DVector2 v1, in DVector2 v2) => (v1 - v2).LengthSquared();

        public static DVector2 operator *(double scale, in DVector2 v) => v.ScaleBy(scale);
        public static DVector2 operator *(in DVector2 v, double scale) => v.ScaleBy(scale);
        public static DVector2 operator /(in DVector2 v, double scale) => v.ScaleBy(1f / scale);
        public DVector2 ScaleBy(double scale) => new DVector2(X * scale, Y * scale);

        public static double Dot(in DVector2 v1, in DVector2 v2) => 
            v1.X * v2.X +
            v1.Y * v2.Y;

        public static double Cross(in DVector2 left, in DVector2 right) => 
            left.X * right.Y -
            left.Y * right.X;

        public static DVector2 Lerp(in DVector2 left, in DVector2 right, double amount) => new DVector2(
            MathHelper.Lerp(left.X, right.X, amount),
            MathHelper.Lerp(left.Y, right.Y, amount));

        public static DVector2 NLerp(in DVector2 left, in DVector2 right, double amount) => 
            Lerp(left, right, amount).Normalize();

        public static DVector2 Min(in DVector2 v1, in DVector2 v2) => new DVector2(
            Math.Min(v1.X, v2.X),
            Math.Min(v1.Y, v2.Y));

        public static DVector2 Max(in DVector2 v1, in DVector2 v2) => new DVector2(
            Math.Max(v1.X, v2.X),
            Math.Max(v1.Y, v2.Y));
        #endregion

        #region Constant Vectors
        public static DVector2 Zero => new DVector2();
        public static DVector2 UnitX => new DVector2(1f, 0f);
        public static DVector2 UnitY => new DVector2(0f, 1f);
        #endregion
    }
}