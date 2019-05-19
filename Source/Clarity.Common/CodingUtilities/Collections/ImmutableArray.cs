using System.Collections;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Collections
{
    public struct ImmutableArray<T> : IReadOnlyList<T>
    {
        public T[] Data { get; }
        public int StartIndex { get; }
        public int Count { get; }

        public ImmutableArray(T[] data) : this(data, 0, data.Length) { }
        public ImmutableArray(T[] data, int startIndex, int count)
        {
            Data = data;
            StartIndex = startIndex;
            Count = count;
        }

        public T this[int index] => Data[StartIndex + index];

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = StartIndex; i < Count; i++)
                yield return Data[i];
        }
    }
}