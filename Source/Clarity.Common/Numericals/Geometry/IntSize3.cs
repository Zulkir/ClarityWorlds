using System.Globalization;

namespace Clarity.Common.Numericals.Geometry
{
    public struct IntSize3
    {
        public int Width;
        public int Height;
        public int Depth;

        public IntSize3(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public int Volume() => Width * Height * Depth;

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return Width ^ (Height << 1) ^ (Depth << 2);
        }

        public override string ToString()
        {
            return
                $"{{ W: {Width.ToString(CultureInfo.InvariantCulture)}; H: {Height.ToString(CultureInfo.InvariantCulture)}; D: {Depth.ToString(CultureInfo.InvariantCulture)} }}";
        }

        public static bool Equals(IntSize3 s1, IntSize3 s2)
        {
            return
                s1.Width == s2.Width &&
                s1.Height == s2.Height &&
                s1.Depth == s2.Depth;
        }

        public static bool operator ==(IntSize3 s1, IntSize3 s2) { return Equals(s1, s2); }
        public static bool operator !=(IntSize3 s1, IntSize3 s2) { return !Equals(s1, s2); }
        public bool Equals(IntSize3 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is IntSize3 && Equals((IntSize3)obj); }
        #endregion
    }
}