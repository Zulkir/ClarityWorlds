using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ByteVector4 : IEquatable<ByteVector4>
    {
        public byte X;
        public byte Y;
        public byte Z;
        public byte W;

        public ByteVector4(byte x, byte y, byte z, byte w)
        {
            X = x; 
            Y = y;
            Z = z;
            W = w;
        }

        public int ToInt() { return X | (Y << 8) | (Z << 16) | (W << 24); }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return ToInt();
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}, {3}}}",
                X.ToString(),
                Y.ToString(),
                Z.ToString(),
                W.ToString());
        }

        public static bool Equals(ByteVector4 s1, ByteVector4 s2)
        {
            return s1.ToInt() == s2.ToInt();
        }

        public static bool operator ==(ByteVector4 s1, ByteVector4 s2) { return Equals(s1, s2); }
        public static bool operator !=(ByteVector4 s1, ByteVector4 s2) { return !Equals(s1, s2); }
        public bool Equals(ByteVector4 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is ByteVector4 && Equals((ByteVector4)obj); }
        #endregion
    }
}
