namespace Clarity.Common.CodingUtilities.Tuples
{
    public static class Tuples
    {
        public static UnorderedPair<T> UnorderedPair<T>(T first, T second) => new UnorderedPair<T>(first, second);
        public static Pair<T> SameTypePair<T>(T first, T second) => new Pair<T>(first, second);
        public static Pair<T1, T2> Pair<T1, T2>(T1 first, T2 second) => new Pair<T1, T2>(first, second);
        public static Tuple3<T> SameTypeTuple<T>(T item0, T item1, T item2) => new Tuple3<T>(item0, item1, item2);
        public static Tuple3<T0, T1, T2> Tuple<T0, T1, T2>(T0 item0, T1 item1, T2 item2) => new Tuple3<T0, T1, T2>(item0, item1, item2);
    }
}