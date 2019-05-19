using System;

namespace Clarity.Common.CodingUtilities.Unmanaged
{
    public struct BinaryDatum16 : IEquatable<BinaryDatum16>
    {
        public long Data0;
        public long Data8;
        
        public bool Equals(BinaryDatum16 other) => Data0 == other.Data0 && Data8 == other.Data8;
        public override bool Equals(object obj) => obj is BinaryDatum16 && Equals((BinaryDatum16)obj);
        public override int GetHashCode() => (Data0.GetHashCode() * 397) ^ Data8.GetHashCode();
        public static bool operator ==(BinaryDatum16 left, BinaryDatum16 right) { return left.Equals(right); }
        public static bool operator !=(BinaryDatum16 left, BinaryDatum16 right) { return !left.Equals(right); }

        public override unsafe string ToString()
        {
            var loc = this;
            var b = (byte*)&loc;
            return $"[{b[0]:X} {b[1]:X} {b[2]:X} {b[3]:X} {b[4]:X} {b[5]:X} {b[6]:X} {b[7]:X} {b[8]:X} {b[9]:X} {b[10]:X} {b[11]:X} {b[12]:X} {b[13]:X} {b[14]:X} {b[15]:X}]";
        }
    }
}