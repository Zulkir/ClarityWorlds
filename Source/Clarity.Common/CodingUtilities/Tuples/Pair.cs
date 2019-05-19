using System;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Tuples
{
    public struct Pair<T> : IEquatable<Pair<T>>
    {
        public T First;
        public T Second;

        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }

        public bool Equals(Pair<T> other) =>
            EqualityComparer<T>.Default.Equals(First, other.First) &&
            EqualityComparer<T>.Default.Equals(Second, other.Second);

        public override bool Equals(object obj) =>
            obj is Pair<T> && Equals((Pair<T>)obj);

        public override int GetHashCode() =>
            (EqualityComparer<T>.Default.GetHashCode(First) * 397) ^ EqualityComparer<T>.Default.GetHashCode(Second);

        public override string ToString() => $"[{First}, {Second}]";

        public static bool operator ==(Pair<T> p1, Pair<T> p2) => p1.Equals(p2);
        public static bool operator !=(Pair<T> p1, Pair<T> p2) => !(p1 == p2);

        public Pair<T> Reverse() => new Pair<T>(Second, First);
    }

    public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
    {
        public T1 First;
        public T2 Second;

        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        public bool Equals(Pair<T1, T2> other) => 
            EqualityComparer<T1>.Default.Equals(First, other.First) && 
            EqualityComparer<T2>.Default.Equals(Second, other.Second);

        public override bool Equals(object obj) => 
            obj is Pair<T1, T2> && Equals((Pair<T1, T2>)obj);

        public override int GetHashCode() => 
            (EqualityComparer<T1>.Default.GetHashCode(First) * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Second);

        public static bool operator ==(Pair<T1, T2> p1, Pair<T1, T2> p2) => p1.Equals(p2);
        public static bool operator !=(Pair<T1, T2> p1, Pair<T1, T2> p2) => !(p1 == p2);

        public Pair<T2, T1> Reverse() => new Pair<T2, T1>(Second, First);

        public static Pair<T1, T2> Create(T1 first, T2 second) => new Pair<T1, T2>(first, second);
    }
}