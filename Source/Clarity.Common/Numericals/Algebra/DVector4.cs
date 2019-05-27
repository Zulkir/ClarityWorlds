using System;
using System.Globalization;

namespace Clarity.Common.Numericals.Algebra
{
    public struct DVector4 : IEquatable<DVector4>
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        #region Constructors
        public DVector4(double x, double y, double z, double w)
        {
            X = x; Y = y; Z = z; W = w;
        }

        //public DVector4(DVector2 xy, double z, double w)
        //{
        //    X = xy.X; Y = xy.Y; Z = z; W = w;
        //}

        public DVector4(in DVector3 xyz, double w)
        {
            X = xyz.X; Y = xyz.Y; Z = xyz.Z; W = w;
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

        public DVector3 Xyz
        {
            get => new DVector3(X, Y, Z);
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

        public static bool Equals(in DVector4 s1, in DVector4 s2)
        {
            return
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z &&
                s1.W == s2.W;
        }

        public static bool operator ==(in DVector4 s1, in DVector4 s2) => Equals(s1, s2);
        public static bool operator !=(in DVector4 s1, in DVector4 s2) => !Equals(s1, s2);
        public bool Equals(DVector4 other) => Equals(this, other);
        public override bool Equals(object obj) => obj is DVector4 && Equals((DVector4)obj);
        #endregion

        #region Math
        public double Length() => Math.Sqrt(LengthSquared());
        public double LengthSquared() => X * X + Y * Y + Z * Z + W * W;

        public DVector4 Normalize()
        {
            var invLength = 1.0 / Length();
            return new DVector4(X * invLength, Y * invLength, Z * invLength, W * invLength);
        }

        public static DVector4 operator -(in DVector4 v) => v.Negate();
        public DVector4 Negate() => new DVector4(-X, -Y, -Z, -W);

        public static DVector4 operator +(in DVector4 v1, in DVector4 v2) => Add(v1, v2);
        public static DVector4 Add(in DVector4 v1, in DVector4 v2)
        {
            return new DVector4(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z,
                v1.W + v2.W);
        }

        public static DVector4 operator -(in DVector4 left, in DVector4 right) => Subtract(left, right);
        public static DVector4 Subtract(in DVector4 left, in DVector4 right)
        {
            return new DVector4(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W);
        }

        public static double Distance(in DVector4 v1, in DVector4 v2) => (v1 - v2).Length();
        public static double DistanceSquared(in DVector4 v1, in DVector4 v2) => (v1 - v2).LengthSquared();

        public static DVector4 operator *(double scale, in DVector4 v) => v.ScaleBy(scale);
        public static DVector4 operator *(in DVector4 v, double scale) => v.ScaleBy(scale);
        public static DVector4 operator /(in DVector4 v, double scale) => v.ScaleBy(1f / scale);
        public DVector4 ScaleBy(double scale) => new DVector4(X * scale, Y * scale, Z * scale, W * scale);

        public static double Dot(in DVector4 v1, in DVector4 v2)
        {
            return 
                v1.X * v2.X + 
                v1.Y * v2.Y + 
                v1.Z * v2.Z + 
                v1.W * v2.W;
        }

        public static DVector4 Lerp(in DVector4 left, in DVector4 right, double amount)
        {
            return new DVector4(
                MathHelper.Lerp(left.X, right.X, amount),
                MathHelper.Lerp(left.Y, right.Y, amount),
                MathHelper.Lerp(left.Z, right.Z, amount),
                MathHelper.Lerp(left.W, right.W, amount));
        }

        public static DVector4 NLerp(in DVector4 left, in DVector4 right, double amount)
        {
            return Lerp(left, right, amount).Normalize();
        }

        public static DVector4 Min(in DVector4 v1, in DVector4 v2)
        {
            return new DVector4(
                Math.Min(v1.X, v2.X),
                Math.Min(v1.Y, v2.Y),
                Math.Min(v1.Z, v2.Z),
                Math.Min(v1.W, v2.W));
        }

        public static DVector4 Max(in DVector4 v1, in DVector4 v2)
        {
            return new DVector4(
                Math.Max(v1.X, v2.X),
                Math.Max(v1.Y, v2.Y),
                Math.Max(v1.Z, v2.Z),
                Math.Max(v1.W, v2.W));
        }
        #endregion

        #region Constant Vectors
        public static DVector4 Zero => new DVector4();
        public static DVector4 UnitX => new DVector4(1f, 0f, 0f, 0f);
        public static DVector4 UnitY => new DVector4(0f, 1f, 0f, 0f);
        public static DVector4 UnitZ => new DVector4(0f, 0f, 1f, 0f);
        public static DVector4 UnitW => new DVector4(0f, 0f, 0f, 1f);
        #endregion
    }
}