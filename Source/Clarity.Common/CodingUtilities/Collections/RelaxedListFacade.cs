using System;
using System.Collections;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Collections
{
    public class RelaxedListFacade<TActual, TRelaxed> : IList<TRelaxed>, IReadOnlyList<TRelaxed>
        where TActual : class, TRelaxed
    {
        private readonly Func<IList<TActual>> getList;

        public int Count => getList().Count;
        public bool IsReadOnly => getList().IsReadOnly;

        public TRelaxed this[int index] { get { return getList()[index]; } set { getList()[index] = EnsureActual(value); } }

        public RelaxedListFacade(Func<IList<TActual>> getList)
        {
            this.getList = getList;
        }

        private static TActual EnsureActual(TRelaxed item)
        {
            var actual = item as TActual;
            if (actual == null && item != null)
                throw new ArgumentException($"Trying to modify a list of '{typeof(TActual).Name}' with an item of type '{item.GetType().Name}'.");
            return actual;
        }

        private TResult IfActual<TResult>(TRelaxed item, Func<IList<TActual>, TActual, TResult> ifIs, TResult ifNot)
        {
            var actual = item as TActual;
            if (actual == null && item != null)
                return ifNot;
            return ifIs(getList(), actual);
        }

        public IEnumerator<TRelaxed> GetEnumerator() => getList().GetEnumerator() as IEnumerator<TRelaxed>;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public bool Contains(TRelaxed item) => IfActual(item, (l, x) => l.Contains(x), false);
        public int IndexOf(TRelaxed item) => IfActual(item, (l, x) => l.IndexOf(x), -1);
        public void Add(TRelaxed item) => getList().Add(EnsureActual(item));
        public void Insert(int index, TRelaxed item) => getList().Insert(index, EnsureActual(item));
        public void RemoveAt(int index) => getList().RemoveAt(index);
        public bool Remove(TRelaxed item) => IfActual(item, (l, x) => l.Remove(x), false);
        public void Clear() => getList().Clear();
        
        public void CopyTo(TRelaxed[] array, int arrayIndex)
        {
            var list = getList();
            for (int i = 0; i < list.Count; i++)
                array[arrayIndex + i] = list[i];
        }
    }
}