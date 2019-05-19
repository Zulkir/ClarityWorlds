using System;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static T[] Copy<T>(this T[] array)
        {
            var result = new T[array.Length];
            Array.Copy(array, result, array.Length);
            return result;
        }

        public static void AddUnique<T>(this ICollection<T> collection, T item)
        {
            if (!collection.Contains(item))
                collection.Add(item);
        }

        public static int? SafeIndexOf<T>(this IList<T> collection, T item)
        {
            var index = collection.IndexOf(item);
            return index >= 0 ? index : (int?)null;
        }
    }
}