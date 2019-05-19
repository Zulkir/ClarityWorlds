using System;
using System.Collections.Concurrent;

namespace Clarity.Common.CodingUtilities.Collections
{
    public class Pool<T>
    {
        private readonly ConcurrentBag<PoolItem<T>> items;
        private readonly Func<T> create;

        public Pool(Func<T> create)
        {
            this.create = create;
            items = new ConcurrentBag<PoolItem<T>>();
        }

        public PoolItem<T> Allocate()
        {
            if (items.TryTake(out var item))
                return item;
            return new PoolItem<T>(this, create());
        }

        public void Return(PoolItem<T> item)
        {
            items.Add(item);
        }
    }
}