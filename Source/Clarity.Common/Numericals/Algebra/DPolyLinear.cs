using System;
using System.Globalization;

namespace Clarity.Common.Numericals.Algebra
{
    public struct DPolyLinear : IEquatable<DPolyLinear>
    {
        public DVector2 Coeffs;

        public double A { get => Coeffs.Y; set => Coeffs.Y = value; }
        public double B { get => Coeffs.X; set => Coeffs.X = value; }
        
        public DPolyLinear(in DVector2 coeffs)
        {
            Coeffs = coeffs;
        }

        public DPolyLinear(double a, double b)
        {
            Coeffs.Y = a;
            Coeffs.X = b;
        }

        public DPolyLinear(double x, double d0, double d1)
        {
            Coeffs.Y = d1;
            Coeffs.X = d0 - d1 * x;
        }

        public double ValueAt(double x) => A * x + B;

        #region Equality, Hash, String
        public override int GetHashCode() => Coeffs.GetHashCode();

        public override string ToString()
        {
            return string.Format("{0}x + {1}",
                A.ToString(CultureInfo.InvariantCulture),
                B.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(in DPolyLinear p1, in DPolyLinear p2) => p1.Coeffs == p2.Coeffs;
        public static bool operator ==(in DPolyLinear p1, in DPolyLinear p2) => Equals(p1, p2);
        public static bool operator !=(in DPolyLinear p1, in DPolyLinear p2) => !Equals(p1, p2);
        public bool Equals(DPolyLinear other) => Equals(this, other);
        public override bool Equals(object obj) => obj is DPolyLinear && Equals((DPolyLinear)obj);
        #endregion

        #region Math
        public static DPolyLinear operator -(in DPolyLinear p) => p.Negate();
        public DPolyLinear Negate() => new DPolyLinear(-Coeffs);

        public static DPolyLinear operator +(in DPolyLinear p1, in DPolyLinear p2) => Add(p1, p2);
        public static DPolyLinear Add(in DPolyLinear p1, in DPolyLinear p2) => new DPolyLinear(p1.Coeffs + p2.Coeffs);

        public static DPolyLinear operator -(in DPolyLinear left, in DPolyLinear right) => Subtract(left, right);
        public static DPolyLinear Subtract(in DPolyLinear left, in DPolyLinear right) => new DPolyLinear(left.Coeffs - right.Coeffs);

        public static DPolyLinear operator *(double scale, in DPolyLinear v) => v.ScaleBy(scale);
        public static DPolyLinear operator *(in DPolyLinear v, float scale) => v.ScaleBy(scale);
        public static DPolyLinear operator /(in DPolyLinear v, float scale) => v.ScaleBy(1f / scale);
        public DPolyLinear ScaleBy(double scale) => new DPolyLinear(Coeffs * scale);

        public static DPolyLinear Lerp(in DPolyLinear left, in DPolyLinear right, double amount) => 
            new DPolyLinear(DVector2.Lerp(left.Coeffs, right.Coeffs, amount));

        public double Derive() => A;

        public double Integrate(double from, double to)
        {
            var sum = 0.0;
            var pFrom = from;
            var pTo = to;
            sum += B * (pTo - pFrom);
            pFrom *= from;
            pTo *= to;
            sum += A * (pTo - pFrom) / 2;
            return sum;
        }
        #endregion
    }
}