using System.Collections;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Collections
{
    public class RelaxedObservableList<T> : IRelaxedObservableList<T>, IReadOnlyList<T>
    {
        private readonly List<T> list;

        public event OnRelaxedObservableListItemEvent<T> ItemAdded;
        public event OnRelaxedObservableListItemEvent<T> ItemRemoved;

        public int Count => list.Count;
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get { return list[index]; }
            set
            {
                list[index] = value;
            }
        }

        public RelaxedObservableList()
        {
            list = new List<T>();
        }
        
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public bool Contains(T item) => list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        public int IndexOf(T item) => list.IndexOf(item);

        public void Add(T item)
        {
            list.Add(item);
            ItemAdded?.Invoke(item, Count - 1);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            ItemAdded?.Invoke(item, index);
        }
        
        public void RemoveAt(int index)
        {
            var item = list[index];
            list.RemoveAt(index);
            ItemRemoved?.Invoke(item, index);
        }

        public bool Remove(T item)
        {
            int index = list.IndexOf(item);
            if (index == -1)
                return false;
            list.RemoveAt(index);
            ItemRemoved?.Invoke(item, index);
            return true;
        }
        
        public void Clear()
        {
            for (int i = Count - 1; i >= 0; i--)
                RemoveAt(i);
        }
    }
}