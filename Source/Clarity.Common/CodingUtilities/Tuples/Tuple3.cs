using System;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Tuples
{
    public struct Tuple3<T> : IEquatable<Tuple3<T>>
    {
        public T Item0;
        public T Item1;
        public T Item2;

        public Tuple3(T item0, T item1, T item2)
        {
            Item0 = item0;
            Item1 = item1;
            Item2 = item2;
        }

        public bool Equals(Tuple3<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Item0, other.Item0) && 
                   EqualityComparer<T>.Default.Equals(Item1, other.Item1) && 
                   EqualityComparer<T>.Default.Equals(Item2, other.Item2);
        }

        public override bool Equals(object obj)
        {
            return obj is Tuple3<T> tuple3 && Equals(tuple3);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(Item0);
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(Item2);
                return hashCode;
            }
        }

        public override string ToString() => $"({Item0}, {Item1}, ({Item2}))";

        public static bool operator ==(Tuple3<T> t1, Tuple3<T> t2) => t1.Equals(t2);
        public static bool operator !=(Tuple3<T> t1, Tuple3<T> t2) => !(t1 == t2);
    }

    public struct Tuple3<T0, T1, T2> : IEquatable<Tuple3<T0, T1, T2>>
    {
        public T0 Item0;
        public T1 Item1;
        public T2 Item2;

        public Tuple3(T0 item0, T1 item1, T2 item2)
        {
            Item0 = item0;
            Item1 = item1;
            Item2 = item2;
        }

        public bool Equals(Tuple3<T0, T1, T2> other)
        {
            return EqualityComparer<T0>.Default.Equals(Item0, other.Item0) && 
                   EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && 
                   EqualityComparer<T2>.Default.Equals(Item2, other.Item2);
        }

        public override bool Equals(object obj)
        {
            return obj is Tuple3<T0, T1, T2> tuple3 && Equals(tuple3);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T0>.Default.GetHashCode(Item0);
                hashCode = (hashCode * 397) ^ EqualityComparer<T1>.Default.GetHashCode(Item1);
                hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Item2);
                return hashCode;
            }
        }

        public override string ToString() => $"({Item0}, {Item1}, ({Item2}))";

        public static bool operator ==(Tuple3<T0, T1, T2> t1, Tuple3<T0, T1, T2> t2) => t1.Equals(t2);
        public static bool operator !=(Tuple3<T0, T1, T2> t1, Tuple3<T0, T1, T2> t2) => !(t1 == t2);
    }
}