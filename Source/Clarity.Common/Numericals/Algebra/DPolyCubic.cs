using System;
using System.Globalization;

namespace Clarity.Common.Numericals.Algebra
{
    public struct DPolyCubic : IEquatable<DPolyCubic>
    {
        public DVector4 Coeffs;

        public double A { get => Coeffs.W; set => Coeffs.W = value; }
        public double B { get => Coeffs.Z; set => Coeffs.Z = value; }
        public double C { get => Coeffs.Y; set => Coeffs.Y = value; }
        public double D { get => Coeffs.X; set => Coeffs.X = value; }
        
        public DPolyCubic(in DVector4 coeffs)
        {
            Coeffs = coeffs;
        }

        public DPolyCubic(double a, double b, double c, double d)
        {
            Coeffs.W = a;
            Coeffs.Z = b;
            Coeffs.Y = c;
            Coeffs.X = d;
        }

        public DPolyCubic(double x, double d0, double d1, double d2, double d3)
        {
            var x2 = x * x;
            var x3 = x2 * x;
            Coeffs.W = d3 / 6;
            Coeffs.Z = (d2 - d3 * x) / 2;
            Coeffs.Y = d1 - d2 * x + d3 * x2 / 2;
            Coeffs.X = d0 - d1 * x + d2 * x2 / 2 - d3 * x3 / 6;
        }

        public double ValueAt(double x)
        {
            var sum = D;
            var arg = x;
            sum += C * arg;
            arg *= x;
            sum += B * arg;
            arg *= x;
            sum += A * arg;
            return sum;
        }

        #region Equality, Hash, String
        public override int GetHashCode() { return Coeffs.GetHashCode(); }

        public override string ToString()
        {
            return string.Format("{0}x3 + {1}x2 + {2}x + {3}",
                A.ToString(CultureInfo.InvariantCulture),
                B.ToString(CultureInfo.InvariantCulture),
                C.ToString(CultureInfo.InvariantCulture),
                D.ToString(CultureInfo.InvariantCulture));
        }

        public string ToStringAt(double x)
        {
            var d1 = this.Derive();
            var d2 = d1.Derive();
            var d3 = d2.Derive();
            return $"d0: {this.ValueAt(x)}, d1: {d1.ValueAt(x)}, d2: {d2.ValueAt(x)}, d3: {d3}";
        }

        public static bool Equals(in DPolyCubic p1, in DPolyCubic p2) => p1.Coeffs == p2.Coeffs;
        public static bool operator ==(in DPolyCubic p1, in DPolyCubic p2) => Equals(p1, p2);
        public static bool operator !=(in DPolyCubic p1, in DPolyCubic p2) => !Equals(p1, p2);
        public bool Equals(DPolyCubic other) => Equals(this, other);
        public override bool Equals(object obj) => obj is DPolyCubic && Equals((DPolyCubic)obj);
        #endregion

        #region Math
        public static DPolyCubic operator -(in DPolyCubic p) => p.Negate();
        public DPolyCubic Negate() => new DPolyCubic(-Coeffs);

        public static DPolyCubic operator +(in DPolyCubic p1, in DPolyCubic p2) => Add(p1, p2);
        public static DPolyCubic Add(in DPolyCubic p1, in DPolyCubic p2) => new DPolyCubic(p1.Coeffs + p2.Coeffs);

        public static DPolyCubic operator -(in DPolyCubic left, in DPolyCubic right) => Subtract(left, right);
        public static DPolyCubic Subtract(in DPolyCubic left, in DPolyCubic right) => new DPolyCubic(left.Coeffs - right.Coeffs);

        public static DPolyCubic operator *(double scale, in DPolyCubic v) => v.ScaleBy(scale);
        public static DPolyCubic operator *(in DPolyCubic v, double scale) => v.ScaleBy(scale);
        public static DPolyCubic operator /(in DPolyCubic v, double scale) => v.ScaleBy(1f / scale);
        public DPolyCubic ScaleBy(double scale) => new DPolyCubic(Coeffs * scale);

        public static DPolyCubic Lerp(in DPolyCubic left, in DPolyCubic right, double amount) => 
            new DPolyCubic(DVector4.Lerp(left.Coeffs, right.Coeffs, amount));

        public DPolyQuadratic Derive() => new DPolyQuadratic(3 * A, 2 * B, C);
        public DPolyCubic DeriveAsCubic() => new DPolyCubic(0, 3 * A, 2 * B, C);

        public double Integrate(double from, double to)
        {
            var sum = 0.0;
            var pFrom = from;
            var pTo = to;
            sum += D * (pTo - pFrom);
            pFrom *= from;
            pTo *= to;
            sum += C * (pTo - pFrom) / 2;
            pFrom *= from;
            pTo *= to;
            sum += B * (pTo - pFrom) / 3;
            pFrom *= from;
            pTo *= to;
            sum += A * (pTo - pFrom) / 4;
            return sum;
        }
        #endregion

        #region Constants
        public static DPolyCubic Zero => new DPolyCubic(0, 0, 0, 0);
        #endregion
    }
}