using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Collections
{
    public class CovariantDictionaryWrapper<TKey, TValueReal, TValueWrap> : IReadOnlyDictionary<TKey, TValueWrap>
        where TValueReal : class, TValueWrap
    {
        private readonly IReadOnlyDictionary<TKey, TValueReal> realDict;

        public CovariantDictionaryWrapper(IReadOnlyDictionary<TKey, TValueReal> realDict)
        {
            this.realDict = realDict;
        }

        public int Count => realDict.Count;
        public bool ContainsKey(TKey key) => realDict.ContainsKey(key);
        public TValueWrap this[TKey key] => realDict[key];
        public IEnumerable<TKey> Keys => realDict.Keys;
        public IEnumerable<TValueWrap> Values => realDict.Values;

        public IEnumerator<KeyValuePair<TKey, TValueWrap>> GetEnumerator() => 
            realDict.Select(realKvp => new KeyValuePair<TKey, TValueWrap>(realKvp.Key, realKvp.Value)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool TryGetValue(TKey key, out TValueWrap value)
        {
            if (realDict.TryGetValue(key, out TValueReal realValue))
            {
                value = realValue;
                return true;
            }
            value = default(TValueWrap);
            return false;
        }
    }
}