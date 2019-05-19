using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.Common.CodingUtilities.Collections
{
    public struct SubList<T> : IReadOnlyList<T>, IList<T>
    {
        public IReadOnlyList<T> Parent { get; }
        public int FirstIndex { get; }
        public int Count { get; }

        public bool IsReadOnly => true;

        public SubList(IReadOnlyList<T> parent, int firstIndex, int count)
        {
            Parent = parent;
            FirstIndex = firstIndex;
            Count = count;
        }

        public T this[int index] => Parent[FirstIndex + index];
        T IList<T>.this[int index]
        {
            get => this[index];
            set => throw new InvalidOperationException();
        }

        public void Add(T item) => throw new InvalidOperationException();
        public void Clear() => throw new InvalidOperationException();
        public bool Remove(T item) => throw new InvalidOperationException();
        public void Insert(int index, T item) => throw new InvalidOperationException();
        public void RemoveAt(int index) => throw new InvalidOperationException();

        public bool Contains(T item) => Enumerable.Contains(this, item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
                array[arrayIndex + i] = this[i];
        }
        
        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < Count; i++)
                if (comparer.Equals(this[i], item))
                    return i;
            return -1;
        }

        public IEnumerator<T> GetEnumerator() => Parent.Skip(FirstIndex).Take(Count).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public T Last() => this[Count - 1];
        public SubList<T> WithNext() => new SubList<T>(Parent, FirstIndex, Count + 1);
        public SubList<T> WithoutLast() => new SubList<T>(Parent, FirstIndex, Count - 1);
    }
}