using System;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Common
{
    public static class ClonableExtensions
    {
        public static T Clone<T>(this ICloneable obj) => (T)obj.Clone();
        public static T CloneTyped<T>(this T obj) where T : ICloneable => (T)obj.Clone();
    }
}