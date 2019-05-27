using System;
using System.Globalization;

namespace Clarity.Common.Numericals.Algebra
{
    public struct DVector3 : IEquatable<DVector3>
    {
        public double X;
        public double Y;
        public double Z;

        #region Constructors
        public DVector3(double value)
        {
            X = Y = Z = value;
        }

        public DVector3(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        //public DVector3(DVector2 xy, double z)
        //{
        //    X = xy.X; Y = xy.Y; Z = z;
        //}
        #endregion

        public double this[int index]
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

        //public DVector2 Xy
        //{
        //    get => new DVector2(X, Y);
        //    set { X = value.X; Y = value.Y; }
        //}
        //
        //public DVector2 Xz
        //{
        //    get => new DVector2(X, Z);
        //    set { X = value.X; Z = value.Y; }
        //}

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

        public static bool Equals(in DVector3 s1, in DVector3 s2)
        {
            return
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z;
        }

        public static bool operator ==(in DVector3 s1, in DVector3 s2) => Equals(s1, s2);
        public static bool operator !=(in DVector3 s1, in DVector3 s2) => !Equals(s1, s2);
        public bool Equals(DVector3 other) => Equals(this, other);
        public override bool Equals(object obj) => obj is DVector3 && Equals((DVector3)obj);
        #endregion

        #region Math
        public double Length() => Math.Sqrt(LengthSquared());
        public double LengthSquared() => X * X + Y * Y + Z * Z;

        public DVector3 Normalize()
        {
            double invLength = 1.0f / Length();
            return new DVector3(X * invLength, Y * invLength, Z * invLength);
        }

        public DVector3 ToPositiveDirection()
        {
            var firstNonZero = X != 0f ? X : Y != 0f ? Y : Z;
            return firstNonZero == 0f
                ? UnitZ :
                (firstNonZero > 0 ? this : -this);
        }

        public DVector3 Abs() => 
            new DVector3(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

        public static DVector3 operator -(in DVector3 v) => v.Negate();
        public DVector3 Negate() => new DVector3(-X, -Y, -Z);

        public static DVector3 operator +(in DVector3 v1, in DVector3 v2) => Add(v1, v2);
        public static DVector3 Add(in DVector3 v1, in DVector3 v2) => new DVector3(
            v1.X + v2.X,
            v1.Y + v2.Y,
            v1.Z + v2.Z);

        public static DVector3 operator -(in DVector3 left, in DVector3 right) => Subtract(left, right);
        public static DVector3 Subtract(in DVector3 left, in DVector3 right) => new DVector3(
            left.X - right.X,
            left.Y - right.Y,
            left.Z - right.Z);

        public static double Distance(in DVector3 v1, in DVector3 v2) => (v1 - v2).Length();
        public static double DistanceSquared(in DVector3 v1, in DVector3 v2) => (v1 - v2).LengthSquared();

        public static DVector3 operator *(double scale, in DVector3 v) => v.ScaleBy(scale);
        public static DVector3 operator *(in DVector3 v, double scale) => v.ScaleBy(scale);
        public static DVector3 operator /(in DVector3 v, double scale) => v.ScaleBy(1f / scale);
        public DVector3 ScaleBy(double scale) => new DVector3(X * scale, Y * scale, Z * scale);

        public static double Dot(in DVector3 v1, in DVector3 v2) => 
             v1.X * v2.X + 
             v1.Y * v2.Y + 
             v1.Z * v2.Z;

        public static DVector3 Cross(in DVector3 left, in DVector3 right) => new DVector3(
            left.Y * right.Z - left.Z * right.Y,
            left.Z * right.X - left.X * right.Z,
            left.X * right.Y - left.Y * right.X);

        public static DVector3 Lerp(in DVector3 left, in DVector3 right, double amount) => new DVector3(
            MathHelper.Lerp(left.X, right.X, amount),
            MathHelper.Lerp(left.Y, right.Y, amount),
            MathHelper.Lerp(left.Z, right.Z, amount));

        public static DVector3 NLerp(in DVector3 left, in DVector3 right, double amount) => 
            Lerp(left, right, amount).Normalize();

        public static DVector3 Min(in DVector3 v1, in DVector3 v2) => new DVector3(
            Math.Min(v1.X, v2.X),
            Math.Min(v1.Y, v2.Y),
            Math.Min(v1.Z, v2.Z));

        public static DVector3 Max(in DVector3 v1, in DVector3 v2) => new DVector3(
            Math.Max(v1.X, v2.X),
            Math.Max(v1.Y, v2.Y),
            Math.Max(v1.Z, v2.Z));
        #endregion

        #region Constant Vectors
        public static DVector3 Zero => new DVector3();
        public static DVector3 UnitX => new DVector3(1f, 0f, 0f);
        public static DVector3 UnitY => new DVector3(0f, 1f, 0f);
        public static DVector3 UnitZ => new DVector3(0f, 0f, 1f);
        #endregion
    }
}