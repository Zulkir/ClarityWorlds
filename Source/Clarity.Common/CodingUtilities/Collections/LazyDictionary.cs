using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.Common.CodingUtilities.Collections
{
    public struct LazyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, TValue> dict;

        public Dictionary<TKey, TValue> Dictionary => 
            dict ?? (dict = new Dictionary<TKey, TValue>());

        public int Count => dict?.Count ?? 0;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => 
            (dict ?? Enumerable.Empty<KeyValuePair<TKey, TValue>>()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}