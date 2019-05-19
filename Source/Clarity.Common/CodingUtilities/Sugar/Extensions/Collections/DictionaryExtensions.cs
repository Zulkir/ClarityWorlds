using System;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue Search<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryGetValue(key, out var value) ? value : default(TValue);
        }

        public static TValue TryGetRef<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : class => 
            dict.TryGetValue(key, out TValue value) ? value : null;

        public static TValue? TryGetVal<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : struct =>
            dict.TryGetValue(key, out TValue value) ? value : (TValue?)null;

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> create)
        {
            if (dict.TryGetValue(key, out var value)) 
                return value;
            value = create(key);
            dict.Add(key, value);
            return value;
        }

        public static TValue AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
            TValue addedValue, Func<TKey, TValue, TValue> update)
        {
            if (!dict.TryGetValue(key, out var existing))
            {
                var value = addedValue;
                dict.Add(key, value);
                return value;
            }
            else
            {
                var value = update(key, existing);
                dict[key] = value;
                return value;
            }
        }

        public static TValue AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, 
            Func<TKey, TValue> create, Func<TKey, TValue, TValue> update)
        {
            TValue existing;
            if (!dict.TryGetValue(key, out existing))
            {
                var value = create(key);
                dict.Add(key, value);
                return value;
            }
            else
            {
                var value = update(key, existing);
                dict[key] = value;
                return value;
            }
        }
    }
}