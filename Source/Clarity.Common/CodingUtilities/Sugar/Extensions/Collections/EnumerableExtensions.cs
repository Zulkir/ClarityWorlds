using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Tuples;
using JetBrains.Annotations;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ExceptSingle<T>(this IEnumerable<T> source, T excpetion)
        {
            var comparer = EqualityComparer<T>.Default;
            using (var e = source.GetEnumerator())
                while (e.MoveNext())
                    if (!comparer.Equals(e.Current, excpetion))
                        yield return e.Current;
        }

        public static IEnumerable<T> ExceptLast<T>(this IEnumerable<T> source)
        {
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                    yield break;
                var last = e.Current;
                while (e.MoveNext())
                {
                    yield return last;
                    last = e.Current;
                }
            }
        }

        public static T? MinOrNull<T>(this IEnumerable<T> source) 
            where T : struct, IComparable<T> => 
            TryFindMinimal(source, x => x, out var item) ? item : (T?)null;

        public static TItem Minimal<TItem, TVal>(this IEnumerable<TItem> source, Func<TItem, TVal> getVal)
            where TVal : IComparable<TVal> => 
            TryFindMinimal(source, getVal, out var minItem) ? minItem : throw new InvalidOperationException("Sequence contains no elements.");

        public static TItem MinimalOrDefault<TItem, TVal>(this IEnumerable<TItem> source, Func<TItem, TVal> getVal)
            where TVal : IComparable<TVal> => 
            TryFindMinimal(source, getVal, out var minItem) ? minItem : default(TItem);

        private static bool TryFindMinimal<TItem, TVal>(IEnumerable<TItem> source, Func<TItem, TVal> getVal, out TItem minItem)
            where TVal : IComparable<TVal>
        {
            minItem = default(TItem);
            var minVal = default(TVal);
            var onlyStarted = true;

            foreach (var item in source)
            {
                var val = getVal(item);
                if (val.CompareTo(minVal) < 0 || onlyStarted)
                {
                    minItem = item;
                    minVal = val;
                    onlyStarted = false;
                }
            }

            return !onlyStarted;
        }

        public static TItem Maximal<TItem, TVal>(this IEnumerable<TItem> source, Func<TItem, TVal> getVal)
            where TVal : IComparable<TVal>
        {
            var minItem = default(TItem);
            var minVal = default(TVal);
            var onlyStarted = true;

            foreach (var item in source)
            {
                var val = getVal(item);
                if (val.CompareTo(minVal) > 0 || onlyStarted)
                {
                    minItem = item;
                    minVal = val;
                    onlyStarted = false;
                }
            }

            if (onlyStarted)
                throw new InvalidOperationException("Sequence contains no elements.");
            return minItem;
        }

        [CanBeNull]
        public static T? FirstOrNull<T>(this IEnumerable<T> source) where T : struct
        {
            using (var e = source.GetEnumerator())
                return e.MoveNext() ? e.Current : (T?)null;
        }

        public static T? LastOrNull<T>(this IEnumerable<T> source) where T : struct
        {
            using (var e = source.GetEnumerator())
            {
                var result = (T?)null;
                while (e.MoveNext())
                    result = e.Current;
                return result;
            }
        }

        public static IEnumerable<KeyValuePair<int, T>> SelectWithIndex<T>(this IEnumerable<T> source) => 
            source.Select((x, i) => new KeyValuePair<int, T>(i, x));

        public static int? IndexOf<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            var index = 0;
            foreach (var item in source)
            {
                if (condition(item))
                    return index;
                index++;
            }
            return null;
        }

        public static IEnumerable<T> ConcatSingle<T>(this IEnumerable<T> source, T item)
        {
            foreach (var elem in source)
                yield return elem;
            yield return item;
        }

        public static IEnumerable<Pair<T>> SequentialPairs<T>(this IEnumerable<T> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;
                var curr = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    var prev = curr;
                    curr = enumerator.Current;
                    yield return new Pair<T>(prev, curr);
                }
            }
        }
    }
}