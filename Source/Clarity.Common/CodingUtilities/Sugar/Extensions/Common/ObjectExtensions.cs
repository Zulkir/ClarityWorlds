using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Common
{
    public static class ObjectExtensions
    {
        [NotNull]
        public static T NotNull<T>(this T x) where T : class 
        {
            if (x == null)
                throw new InvalidDataException("Null was not expected here.");
            return x;
        }
        public static IEnumerable<T> EnumSelf<T>(this T obj) { yield return obj; }
        public static IEnumerable<T> EnumSelfAs<T>(this object obj) { yield return (T)obj; }

        public delegate void ValueChange<T>(ref T val);
        public static T With<T>(this T val, ValueChange<T> change) where T : struct
        {
            change(ref val);
            return val;
        }
    }
}