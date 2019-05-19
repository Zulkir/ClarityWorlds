using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 4)]
    public struct ShortVector2 : IEquatable<ShortVector2>
    {
        public short X;
        public short Y;

        public ShortVector2(short x, short y)
        {
            X = x;
            Y = y;
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return X ^ (Y << 16);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}",
                X.ToString(CultureInfo.InvariantCulture),
                Y.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(ShortVector2 s1, ShortVector2 s2)
        {
            return
                s1.X == s2.X &&
                s1.Y == s2.Y;
        }

        public static bool operator ==(ShortVector2 s1, ShortVector2 s2) { return Equals(s1, s2); }
        public static bool operator !=(ShortVector2 s1, ShortVector2 s2) { return !Equals(s1, s2); }
        public bool Equals(ShortVector2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is ShortVector2 && Equals((ShortVector2)obj); }
        #endregion
    }
}
