using System;

namespace Clarity.Common.CodingUtilities.Unmanaged
{
    public struct BinaryDatum32 : IEquatable<BinaryDatum32>
    {
        public BinaryDatum16 Data0;
        public BinaryDatum16 Data16;

        public bool Equals(BinaryDatum32 other) => Data0.Equals(other.Data0) && Data16.Equals(other.Data16);
        public override bool Equals(object obj) => obj is BinaryDatum32 && Equals((BinaryDatum32)obj);
        public override int GetHashCode() => (Data0.GetHashCode() * 397) ^ Data16.GetHashCode();
        public static bool operator ==(BinaryDatum32 left, BinaryDatum32 right) { return left.Equals(right); }
        public static bool operator !=(BinaryDatum32 left, BinaryDatum32 right) { return !left.Equals(right); }
        public override string ToString() => $"[{Data0} {Data16}]";
    }
}