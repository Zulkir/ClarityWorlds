using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.Common.CodingUtilities.Collections
{
    public class FixedList<T> : IList<T>, IReadOnlyList<T>
    {
        private readonly List<T> items;

        public int Count => items.Count;
        public bool IsReadOnly => true;

        public T this[int index]
        {
            get { return items[index]; }
            set { throw new InvalidOperationException("Tring to modify a fixed list."); }
        }

        public FixedList() { items = new List<T>(); }
        public FixedList(IEnumerable<T> items) { this.items = items.ToList(); }

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator() as IEnumerator<T>;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public bool Contains(T item) => items.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);
        public int IndexOf(T item) => items.IndexOf(item);

        public void Add(T item) { throw new InvalidOperationException("Tring to modify a fixed list."); }
        public void Clear() { throw new InvalidOperationException("Tring to modify a fixed list."); }
        public bool Remove(T item) { throw new InvalidOperationException("Tring to modify a fixed list."); }
        public void Insert(int index, T item) { throw new InvalidOperationException("Tring to modify a fixed list."); }
        public void RemoveAt(int index) { throw new InvalidOperationException("Tring to modify a fixed list."); }
    }
}