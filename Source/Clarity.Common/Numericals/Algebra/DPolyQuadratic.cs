using System;
using System.Globalization;

namespace Clarity.Common.Numericals.Algebra
{
    public struct DPolyQuadratic : IEquatable<DPolyQuadratic>
    {
        public DVector3 Coeffs;

        public double A { get => Coeffs.Z; set => Coeffs.Z = value; }
        public double B { get => Coeffs.Y; set => Coeffs.Y = value; }
        public double C { get => Coeffs.X; set => Coeffs.X = value; }
        
        public DPolyQuadratic(in DVector3 coeffs)
        {
            Coeffs = coeffs;
        }

        public DPolyQuadratic(double a, double b, double c)
        {
            Coeffs.Z = a;
            Coeffs.Y = b;
            Coeffs.X = c;
        }

        public DPolyQuadratic(double x, double d0, double d1, double d2)
        {
            var x2 = x * x;
            Coeffs.Z = d2 / 2;
            Coeffs.Y = d1 - d2 * x;
            Coeffs.X = d0 - d1 * x + d2 * x2 / 2;
        }

        public double ValueAt(double x)
        {
            var sum = C;
            var arg = x;
            sum += B * arg;
            arg *= x;
            sum += A * arg;
            return sum;
        }

        #region Equality, Hash, String
        public override int GetHashCode() { return Coeffs.GetHashCode(); }

        public override string ToString()
        {
            return string.Format("{0}x2 + {1}x + {2}",
                A.ToString(CultureInfo.InvariantCulture),
                B.ToString(CultureInfo.InvariantCulture),
                C.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(in DPolyQuadratic p1, in DPolyQuadratic p2) => p1.Coeffs == p2.Coeffs;
        public static bool operator ==(in DPolyQuadratic p1, in DPolyQuadratic p2) => Equals(p1, p2);
        public static bool operator !=(in DPolyQuadratic p1, in DPolyQuadratic p2) => !Equals(p1, p2);
        public bool Equals(DPolyQuadratic other) => Equals(this, other);
        public override bool Equals(object obj) => obj is DPolyQuadratic && Equals((DPolyQuadratic)obj);
        #endregion

        #region Math
        public static DPolyQuadratic operator -(in DPolyQuadratic p) => p.Negate();
        public DPolyQuadratic Negate() => new DPolyQuadratic(-Coeffs);

        public static DPolyQuadratic operator +(in DPolyQuadratic p1, in DPolyQuadratic p2) => Add(p1, p2);
        public static DPolyQuadratic Add(in DPolyQuadratic p1, in DPolyQuadratic p2) => new DPolyQuadratic(p1.Coeffs + p2.Coeffs);

        public static DPolyQuadratic operator -(in DPolyQuadratic left, in DPolyQuadratic right) => Subtract(left, right);
        public static DPolyQuadratic Subtract(in DPolyQuadratic left, in DPolyQuadratic right) => new DPolyQuadratic(left.Coeffs - right.Coeffs);

        public static DPolyQuadratic operator *(double scale, in DPolyQuadratic v) => v.ScaleBy(scale);
        public static DPolyQuadratic operator *(in DPolyQuadratic v, float scale) => v.ScaleBy(scale);
        public static DPolyQuadratic operator /(in DPolyQuadratic v, float scale) => v.ScaleBy(1f / scale);
        public DPolyQuadratic ScaleBy(double scale) => new DPolyQuadratic(Coeffs * scale);

        public static DPolyQuadratic Lerp(in DPolyQuadratic left, in DPolyQuadratic right, double amount) => 
            new DPolyQuadratic(DVector3.Lerp(left.Coeffs, right.Coeffs, amount));

        public DPolyLinear Derive() => new DPolyLinear(2 * A, B);

        public double Integrate(double from, double to)
        {
            var sum = 0.0;
            var pFrom = from;
            var pTo = to;
            sum += C * (pTo - pFrom);
            pFrom *= from;
            pTo *= to;
            sum += B * (pTo - pFrom) / 2;
            pFrom *= from;
            pTo *= to;
            sum += A * (pTo - pFrom) / 3;
            return sum;
        }
        #endregion
    }
}