using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct IntRectangle2 : IEquatable<IntRectangle2>
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public IntRectangle2(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return X ^ (Y << 8) ^ (Width << 16) ^ (Height << 24);
        }

        public override string ToString()
        {
            return
                string.Format("{{ X: {0}; Y: {1}; Width: {2}; Height: {3} }}", 
                    X.ToString(CultureInfo.InvariantCulture),
                    Y.ToString(CultureInfo.InvariantCulture),
                    Width.ToString(CultureInfo.InvariantCulture),
                    Height.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(IntRectangle2 s1, IntRectangle2 s2)
        {
            return 
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Width == s2.Width &&
                s1.Height == s2.Height;
        }

        public static bool operator ==(IntRectangle2 s1, IntRectangle2 s2) { return Equals(s1, s2); }
        public static bool operator !=(IntRectangle2 s1, IntRectangle2 s2) { return !Equals(s1, s2); }
        public bool Equals(IntRectangle2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is IntRectangle2 && Equals((IntRectangle2)obj); }
        #endregion

        public bool ContainsPoint(int x, int y)
        {
            return
                (x >= X) && (x <= X + Width) &&
                (y >= Y) && (y <= Y + Height);
        }
    }
}
