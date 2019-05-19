using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 8)]
    public struct ShortVector4 : IEquatable<ShortVector4>
    {
        public short X;
        public short Y;
        public short Z;
        public short W;

        public ShortVector4(short x, short y, short z, short w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return X ^ (Y << 8) ^ (Z << 16) ^ (W << 24);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}, {3}}}",
                X.ToString(CultureInfo.InvariantCulture),
                Y.ToString(CultureInfo.InvariantCulture),
                Z.ToString(CultureInfo.InvariantCulture),
                W.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(ShortVector4 s1, ShortVector4 s2)
        {
            return
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z &&
                s1.W == s2.W;
        }

        public static bool operator ==(ShortVector4 s1, ShortVector4 s2) { return Equals(s1, s2); }
        public static bool operator !=(ShortVector4 s1, ShortVector4 s2) { return !Equals(s1, s2); }
        public bool Equals(ShortVector4 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is ShortVector4 && Equals((ShortVector4)obj); }
        #endregion
    }
}
