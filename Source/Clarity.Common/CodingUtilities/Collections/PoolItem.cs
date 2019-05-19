using System;

namespace Clarity.Common.CodingUtilities.Collections
{
    public struct PoolItem<T> : IDisposable
    {
        private readonly Pool<T> pool;
        public T Item { get; }

        public PoolItem(Pool<T> pool, T item)
        {
            this.pool = pool;
            Item = item;
        }

        public void Dispose()
        {
            pool.Return(this);
        }
    }
}