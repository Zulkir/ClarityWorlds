using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 8)]
    public struct IntSize2 : IEquatable<IntSize2>
    {
        public int Width;
        public int Height;

        public IntSize2(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Area => Width * Height;

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return Width ^ (Height << 1);
        }

        public override string ToString()
        {
            return
                $"{{ W: {Width.ToString(CultureInfo.InvariantCulture)}; H: {Height.ToString(CultureInfo.InvariantCulture)} }}";
        }

        public static bool Equals(IntSize2 s1, IntSize2 s2)
        {
            return 
                s1.Width == s2.Width &&
                s1.Height == s2.Height;
        }

        public static bool operator ==(IntSize2 s1, IntSize2 s2) { return Equals(s1, s2); }
        public static bool operator !=(IntSize2 s1, IntSize2 s2) { return !Equals(s1, s2); }
        public bool Equals(IntSize2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is IntSize2 && Equals((IntSize2)obj); }
        #endregion
    }
}
