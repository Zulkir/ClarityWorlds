using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 8)]
    public struct Rational : IEquatable<Rational>
    {
        private readonly int numerator;
        private readonly int denominator;

        public Rational(int numerator, int denominator)
        {
            int gcd = Gcd(numerator, denominator);
            this.numerator = numerator / gcd;
            this.denominator = denominator / gcd;
        }

        public int Numerator { get { return numerator; }}
        public int Denominator { get { return denominator; }}

        public float ToSingle() { return (float)numerator / denominator; }
        public double ToDouble() { return (double)numerator / denominator; }
        public int Round() { return (int)System.Math.Round(ToDouble()); }
        public bool IsWhole { get { return denominator == 1; } }

        public static int Gcd(int x, int y)
        {
            while (y != 0)
            {
                int t = y;
                y = x % y;
                x = t;
            }
            return x;
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return numerator ^ (denominator << 8);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}",
                numerator.ToString(CultureInfo.InvariantCulture),
                denominator.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(Rational s1, Rational s2)
        {
            return
                s1.numerator == s2.numerator &&
                s1.denominator == s2.denominator;
        }

        public static bool operator ==(Rational s1, Rational s2) { return Equals(s1, s2); }
        public static bool operator !=(Rational s1, Rational s2) { return !Equals(s1, s2); }
        public bool Equals(Rational other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Rational && Equals((Rational)obj); }
        #endregion

        #region Math
        public static Rational operator -(Rational x) { return x.Negate(); }
        public Rational Negate() { return new Rational(-numerator, denominator); }

        public static Rational operator +(Rational x, Rational y) { return Add(x, y); }
        public static Rational Add(Rational x, Rational y)
        {
            var num = x.numerator * y.denominator + y.numerator * x.denominator;
            var denom = x.denominator * y.denominator;
            return new Rational(num, denom);
        }

        public static Rational operator -(Rational x, Rational y) { return Subtract(x, y); }
        public static Rational Subtract(Rational x, Rational y)
        {
            var num = x.numerator * y.denominator - y.numerator * x.denominator;
            var denom = x.denominator * y.denominator;
            return new Rational(num, denom);
        }

        public static Rational operator *(Rational x, Rational y) { return Multiply(x, y); }
        public static Rational Multiply(Rational x, Rational y)
        {
            var num = x.numerator * y.numerator;
            var denom = x.denominator * y.denominator;
            return new Rational(num, denom);
        }

        public static Rational operator /(Rational x, Rational y) { return Divide(x, y); }
        public static Rational Divide(Rational x, Rational y)
        {
            var num = x.numerator * y.denominator;
            var denom = x.denominator * y.numerator;
            return new Rational(num, denom);
        }
        #endregion
    }
}
