using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Clarity.Common.CodingUtilities.Comparers
{
    public class ReferenceEqualityComparer : IEqualityComparer<object>, IEqualityComparer
    {
        bool IEqualityComparer<object>.Equals(object x, object y) => ReferenceEquals(x, y);
        int IEqualityComparer<object>.GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);

        bool IEqualityComparer.Equals(object x, object y) => ReferenceEquals(x, y);
        int IEqualityComparer.GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);

        public static ReferenceEqualityComparer Singleton { get; } = new ReferenceEqualityComparer();
    }
}