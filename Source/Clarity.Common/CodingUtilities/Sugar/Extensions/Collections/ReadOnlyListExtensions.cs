using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Collections
{
    public static class ReadOnlyListExtensions
    {
        public static int IndexOf<T>(this IReadOnlyList<T> list, T element, IEqualityComparer<T> equalityComparer = null)
        {
            var actualComparer = equalityComparer ?? EqualityComparer<T>.Default;
            for (int i = 0; i < list.Count; i++)
                if (actualComparer.Equals(list[i], element))
                    return i;
            return -1;
        }

        public static IEnumerable<Pair<T>> AllPairs<T>(this IReadOnlyList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            for (int j = i + 1; j < list.Count; j++)
                yield return new Pair<T>(list[i], list[j]);
        }
    }
}