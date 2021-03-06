﻿using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Collections
{
    public static class ReadOnlyListExtensions
    {
        public static bool HasItems<T>(this IReadOnlyList<T> list) => list.Count != 0;
        public static bool IsEmpty<T>(this IReadOnlyList<T> list) => list.Count == 0;

        public static int IndexOf<T>(this IReadOnlyList<T> list, T element, IEqualityComparer<T> equalityComparer = null)
        {
            var actualComparer = equalityComparer ?? EqualityComparer<T>.Default;
            for (var i = 0; i < list.Count; i++)
                if (actualComparer.Equals(list[i], element))
                    return i;
            return -1;
        }

        public static IEnumerable<Pair<T>> AllPairs<T>(this IReadOnlyList<T> list)
        {
            for (var i = 0; i < list.Count - 1; i++)
            for (var j = i + 1; j < list.Count; j++)
                yield return new Pair<T>(list[i], list[j]);
        }
    }
}