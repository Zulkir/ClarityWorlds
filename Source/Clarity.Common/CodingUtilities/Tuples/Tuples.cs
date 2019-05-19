namespace Clarity.Common.CodingUtilities.Tuples
{
    public static class Tuples
    {
        public static UnorderedPair<T> UnorderedPair<T>(T first, T second) => new UnorderedPair<T>(first, second);
        public static Pair<T> SameTypePair<T>(T first, T second) => new Pair<T>(first, second);
        public static Pair<T1, T2> Pair<T1, T2>(T1 first, T2 second) => new Pair<T1, T2>(first, second);
    }
}