using System.Globalization;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    public struct Size3
    {
        public float Width;
        public float Height;
        public float Depth;

        public Size3(float width, float height, float depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public Size3(Vector3 sizeVector)
        {
            Width = sizeVector.X;
            Height = sizeVector.Y;
            Depth = sizeVector.Z;
        }

        public float Volume() => Width * Height * Depth;

        public Vector3 ToVector() => new Vector3(Width, Height, Depth);

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ (Height.GetHashCode() << 1) ^ (Depth.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return
                $"{{ W: {Width.ToString(CultureInfo.InvariantCulture)}; H: {Height.ToString(CultureInfo.InvariantCulture)}; D: {Depth.ToString(CultureInfo.InvariantCulture)} }}";
        }

        public static bool Equals(Size3 s1, Size3 s2)
        {
            return
                s1.Width == s2.Width &&
                s1.Height == s2.Height &&
                s1.Depth == s2.Depth;
        }

        public static bool operator ==(Size3 s1, Size3 s2) { return Equals(s1, s2); }
        public static bool operator !=(Size3 s1, Size3 s2) { return !Equals(s1, s2); }
        public bool Equals(Size3 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Size3 && Equals((Size3)obj); }
        #endregion
    }
}