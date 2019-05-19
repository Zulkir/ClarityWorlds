using System;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Collections
{
    public static class ListExtensions
    {
        public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> condition)
        {
            var deathNote = new Stack<int>();
            for (int i = 0; i < list.Count; i++)
                if (condition(list[i]))
                    deathNote.Push(i);
            while (deathNote.Count > 0)
                list.RemoveAt(deathNote.Pop());
        }
    }
}