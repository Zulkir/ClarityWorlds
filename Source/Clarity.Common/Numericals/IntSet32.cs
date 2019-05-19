using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Clarity.Common.Numericals
{
    public struct IntSet32 : IEquatable<IntSet32>, IEnumerable<int>
    {
        private readonly int raw;

        public IntSet32(int raw)
        {
            this.raw = raw;
        }

        public bool Contains(int index)
        {
            return (raw & (1 << index)) != 0;
        }

        #region Equality, Hash, String
        public override int GetHashCode() { return raw; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 32; i++)
                builder.Append(Contains(i) ? '1' : '0');
            return builder.ToString();
        }

        

        public static bool Equals(IntSet32 s1, IntSet32 s2) { return s1.raw == s2.raw; }
        public static bool operator ==(IntSet32 s1, IntSet32 s2) { return Equals(s1, s2); }
        public static bool operator !=(IntSet32 s1, IntSet32 s2) { return !Equals(s1, s2); }
        public bool Equals(IntSet32 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is IntSet32 && Equals((IntSet32)obj); }
        #endregion

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < 32; i++)
                if (Contains(i))
                    yield return i;
        }

        public bool IsEmpty() { return raw == 0; }

        public int First()
        {
            for (int i = 0; i < 32; i++)
                if (Contains(i))
                    return i;
            throw new InvalidOperationException("Set is empty");
        }

        public int Last()
        {
            int windowOffset = (raw >> 8) == 0 ? 0
                             : (raw >> 16) == 0 ? 8
                             : (raw >> 24) == 0 ? 16
                             : 24;
            int offset = windowOffset + 7;
            for (int i = offset; i >= windowOffset; i--)
                if (Contains(i))
                    return i;
            throw new InvalidOperationException("Set is empty");
        }

        public IntSet32 With(int index)
        {
            return new IntSet32(raw | (1 << index));
        }

        public IntSet32 Without(int index)
        {
            return new IntSet32(raw & ~(1 << index));
        }

        public IntSet32 Union(IntSet32 other)
        {
            return new IntSet32(raw | other.raw);
        }

        public IntSet32 Intersection(IntSet32 other)
        {
            return new IntSet32(raw & other.raw);
        }

        public static IntSet32 Empty()
        {
            return new IntSet32(0);
        }

        public static IntSet32 Range(int start, int length)
        {
            return new IntSet32(((1 << length) - 1) << start);
        }
    }
}