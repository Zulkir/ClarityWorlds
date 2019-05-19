using System;

namespace Clarity.Common.CodingUtilities.Unmanaged
{
    public struct BinaryDatum64 : IEquatable<BinaryDatum64>
    {
        public BinaryDatum32 Data0;
        public BinaryDatum32 Data32;

        public bool Equals(BinaryDatum64 other) => Data0.Equals(other.Data0) && Data32.Equals(other.Data32);
        public override bool Equals(object obj) => obj is BinaryDatum64 && Equals((BinaryDatum64)obj);
        public override int GetHashCode() => (Data0.GetHashCode() * 397) ^ Data32.GetHashCode();
        public static bool operator ==(BinaryDatum64 left, BinaryDatum64 right) { return left.Equals(right); }
        public static bool operator !=(BinaryDatum64 left, BinaryDatum64 right) { return !left.Equals(right); }
        public override string ToString() => $"[{Data0} {Data32}]";
    }
}