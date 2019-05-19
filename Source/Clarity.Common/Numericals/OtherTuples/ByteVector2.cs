using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ByteVector2 : IEquatable<ByteVector2>
    {
        public byte X;
        public byte Y;

        public ByteVector2(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public ushort ToUshort() { return (ushort)ToInt(); }
        public int ToInt() { return X | (Y << 8); }

        #region Equality, Hash, String

        public override int GetHashCode()
        {
            return ToInt();
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}",
                X.ToString(CultureInfo.InvariantCulture),
                Y.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Equals(ByteVector2 s1, ByteVector2 s2)
        {
            return 
                s1.X == s2.X &&
                s1.Y == s2.Y;
        }

        public static bool operator ==(ByteVector2 s1, ByteVector2 s2) { return Equals(s1, s2); }
        public static bool operator !=(ByteVector2 s1, ByteVector2 s2) { return !Equals(s1, s2); }
        public bool Equals(ByteVector2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is ByteVector2 && Equals((ByteVector2)obj); }

        #endregion

    }
}
