using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 8)]
    public struct Size2 : IEquatable<Size2>
    {
        public float Width;
        public float Height;

        public Size2(float width, float height)
        {
            Width = width;
            Height = height;
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ (Height.GetHashCode() << 1);
        }

        public override string ToString()
        {
            return
                $"{{ W: {Width.ToString(CultureInfo.InvariantCulture)}; H: {Height.ToString(CultureInfo.InvariantCulture)} }}";
        }

        public static bool Equals(Size2 s1, Size2 s2)
        {
            return
                s1.Width == s2.Width &&
                s1.Height == s2.Height;
        }

        public static bool operator ==(Size2 s1, Size2 s2) { return Equals(s1, s2); }
        public static bool operator !=(Size2 s1, Size2 s2) { return !Equals(s1, s2); }
        public bool Equals(Size2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Size2 && Equals((Size2)obj); }
        #endregion

        public Vector2 ToVector() => new Vector2(Width, Height);
    }
}