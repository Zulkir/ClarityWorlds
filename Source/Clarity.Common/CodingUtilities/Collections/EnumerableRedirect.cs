using System.Collections;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Collections
{
    public class EnumerableRedirect<T> : IEnumerable<T>
    {
        public IEnumerable<T> Actual { get; set; }
        public IEnumerator<T> GetEnumerator() => Actual.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}