using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.Common.CodingUtilities.Collections
{
    public class EmptyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => 0;
        public bool ContainsKey(TKey key) => false;
        public TValue this[TKey key] => throw new KeyNotFoundException();
        public IEnumerable<TKey> Keys => Enumerable.Empty<TKey>();
        public IEnumerable<TValue> Values => Enumerable.Empty<TValue>();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            yield break;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            return false;
        }

        public static readonly EmptyDictionary<TKey, TValue> Singleton = new EmptyDictionary<TKey, TValue>();
    }
}