using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.Common.Numericals
{
    public struct IntSet : IEnumerable<int>, ICloneable
    {
        private readonly bool[] presence;

        public IntSet(int capacity)
        {
            presence = new bool[capacity];
        }

        public bool Contains(int index)
        {
            return presence[index];
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < presence.Length; i++)
                if (Contains(i))
                    yield return i;
        }

        public object Clone()
        {
            var clone = new IntSet(presence.Length);
            Array.Copy(presence, clone.presence, presence.Length);
            return clone;
        }

        public bool IsEmpty() { return !presence.Any(); }

        public int First()
        {
            for (int i = 0; i < presence.Length; i++)
                if (Contains(i))
                    return i;
            throw new InvalidOperationException("Set is empty");
        }

        public int Last()
        {
            for (int i = presence.Length - 1; i >= 0; i--)
                if (presence[i])
                    return i;
            throw new InvalidOperationException("Set is empty");
        }

        public void Add(int index)
        {
            presence[index] = true;
        }

        public void Add(IntSet other)
        {
            for (int i = 0; i < Math.Min(presence.Length, other.presence.Length); i++)
                presence[i] |= other.presence[i];
        }

        public void Remove(int index)
        {
            presence[index] = false;
        }
    }
}
