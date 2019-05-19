using System;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Tuples
{
    public struct UnorderedPair<T> : IEquatable<UnorderedPair<T>>
    {
        public T First;
        public T Second;

        public UnorderedPair(T first, T second)
        {
            First = first;
            Second = second;
        }

        public bool Contains(T item) => EqualityComparer<T>.Default.Equals(First, item) ||
                                        EqualityComparer<T>.Default.Equals(Second, item);

        public bool Equals(UnorderedPair<T> other) => (EqualityComparer<T>.Default.Equals(First, other.First) &&
                                                       EqualityComparer<T>.Default.Equals(Second, other.Second)) ||
                                                      (EqualityComparer<T>.Default.Equals(First, other.Second) &&
                                                       EqualityComparer<T>.Default.Equals(Second, other.First));

        public override bool Equals(object obj) => obj is UnorderedPair<T> && Equals((UnorderedPair<T>)obj);
        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(First) ^ EqualityComparer<T>.Default.GetHashCode(Second);
        public override string ToString() => $"[{First}, {Second}]";

        public static bool operator ==(UnorderedPair<T> p1, UnorderedPair<T> p2) => p1.Equals(p2);
        public static bool operator !=(UnorderedPair<T> p1, UnorderedPair<T> p2) => !(p1 == p2);
    }
}