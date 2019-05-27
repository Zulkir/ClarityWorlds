using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding.Diffs;

namespace Clarity.Common.Infra.TreeReadWrite.DiffBuilding 
{
    public class TrwDiffBuilder : ITrwDiffBuilder 
    {
        private class Context
        {
            public ITrwDiffIdentityComparer IdentityComparer { get; }
            public Dictionary<Pair<object>, bool> EqualityCache { get; }
            public Dictionary<object, int> SingleComplexityCache { get; }
            public Dictionary<Pair<object>, int> PairComplexityCache { get; }

            public Context(ITrwDiffIdentityComparer identityComparer)
            {
                IdentityComparer = identityComparer;
                EqualityCache = new Dictionary<Pair<object>, bool>();
                SingleComplexityCache = new Dictionary<object, int>();
                PairComplexityCache = new Dictionary<Pair<object>, int>();
            }
        }

        public ITrwDiff BuildDiffs(object oldDynValue, object newDynValue,
                                   ITrwDiffIdentityComparer identityComparer)
        {
            var context = new Context(identityComparer);
            return Diff(context, oldDynValue, newDynValue) ?? new EmptyTrwDiff();
        }

        private static ITrwDiff Diff(Context context, object v1, object v2)
        {
            if (v1 is IDictionary<string, object> o1 && v2 is IDictionary<string, object> o2 && AreSameObject(context, o1, o2))
                return DiffObject(context, o1, o2);
            if (v1 is IList<object> a1 && v2 is IList<object> a2)
                return DiffArray(context, a1, a2);
            if (AreEqual(context, v1, v2))
                return null;
            return new ReplaceValueTrwDiff(v1, v2);
        }

        private static int GetDiffComplexity(Context context, object v1, object v2)
        {
            if (v1 is IDictionary<string, object> o1 && v2 is IDictionary<string, object> o2 && AreSameObject(context, o1, o2))
                return GetDiffComplexityOfObject(context, o1, o2);
            if (v1 is IList<object> a1 && v2 is IList<object> a2)
                return GetDiffComplexityOfArray(context, a1, a2);
            if (AreEqual(context, v1, v2))
                return 0;
            return GetDiffComplexityOfNew(context, v1) + GetDiffComplexityOfNew(context, v2);
        }

        private static ITrwDiff DiffObject(Context context, IDictionary<string, object> oldObject, IDictionary<string, object> newObject)
        {
            var propertiesAdded = newObject.Keys.Where(x => !oldObject.ContainsKey(x)).Select(x => Tuples.Pair(x, newObject[x])).ToArray();
            var propertiesRemoved = oldObject.Keys.Where(x => !newObject.ContainsKey(x)).Select(x => Tuples.Pair(x, oldObject[x])).ToArray();
            var propertiesDiffed = new List<Pair<string, ITrwDiff>>();
            foreach (var commonKey in oldObject.Keys.Where(newObject.ContainsKey))
            {
                var oldValue = oldObject[commonKey];
                var newValue = newObject[commonKey];
                var diff = Diff(context, oldValue, newValue);
                if (diff != null)
                    propertiesDiffed.Add(Tuples.Pair(commonKey, diff));
            }
            return propertiesAdded.Any() || propertiesRemoved.Any() || propertiesDiffed.Any() 
                ? new MutateObjectTrwDiff(propertiesAdded, propertiesRemoved, propertiesDiffed)
                : null;
        }

        private static int GetDiffComplexityOfObject(Context context, IDictionary<string, object> o1, IDictionary<string, object> o2)
        {
            return context.PairComplexityCache.GetOrAdd(new Pair<object>(o1, o2), x => CalcDiffComplexityOfObject(context, o1, o2));
        }

        private static int CalcDiffComplexityOfObject(Context context, IDictionary<string, object> o1, IDictionary<string, object> o2)
        {
            var addedKeyComplexity = o2.Keys.Where(x => !o1.ContainsKey(x)).Sum(x => GetDiffComplexityOfNew(context, o2[x]));
            var removedKeyComplexity = o1.Keys.Where(x => !o2.ContainsKey(x)).Sum(x => GetDiffComplexityOfNew(context, o1[x]));
            var commonKeyComplexity = o1.Keys.Where(o2.ContainsKey).Sum(x => GetDiffComplexity(context, o1[x], o2[x]));
            return addedKeyComplexity + removedKeyComplexity + commonKeyComplexity;
        }

        private static ITrwDiff DiffArray(Context context, IList<object> a1, IList<object> a2)
        {
            var permutation = BuildArrayPermutation(context, a1, a2);
            var addedIndices = Enumerable.Range(0, a2.Count).Where(x => !permutation.Any(y => y.Second == x)).Select(x => Tuples.Pair(x, a2[x])).ToArray();
            var removedIndices = Enumerable.Range(0, a1.Count).Where(x => !permutation.Any(y => y.First == x)).Select(x => Tuples.Pair(x, a1[x])).ToArray();
            var itemsMoved = new List<Pair<int>>();
            var itemsDiffed = new List<Pair<Pair<int>, ITrwDiff>>();
            var unaffectedIndices = new List<int>();
            foreach (var indexPair in permutation)
            {
                var oldValue = a1[indexPair.First];
                var newValue = a2[indexPair.Second];
                var diff = Diff(context, oldValue, newValue);
                if (diff != null)
                    itemsDiffed.Add(Tuples.Pair(indexPair, diff));
                else if (indexPair.First != indexPair.Second)
                    itemsMoved.Add(indexPair);
                else
                    unaffectedIndices.Add(indexPair.First);
            }
            return addedIndices.Any() || removedIndices.Any() || itemsMoved.Any() || itemsDiffed.Any() 
                ? new MutateArrayTrwDiff(addedIndices, removedIndices, itemsMoved, itemsDiffed, unaffectedIndices)
                : null;
        }

        private static int GetDiffComplexityOfArray(Context context, IList<object> a1, IList<object> a2)
        {
            return context.PairComplexityCache.GetOrAdd(new Pair<object>(a1, a2), x => CalcDiffComplexityOfArray(context, a1, a2));
        }

        private static int CalcDiffComplexityOfArray(Context context, IList<object> a1, IList<object> a2)
        {
            var permutation = BuildArrayPermutation(context, a1, a2);
            var addedIndexComplexity = Enumerable.Range(0, a2.Count).Where(x => !permutation.Any(y => y.Second == x)).Sum(x => GetDiffComplexityOfNew(context, a2[x]));
            var removedIndexComplexity = Enumerable.Range(0, a1.Count).Where(x => !permutation.Any(y => y.First == x)).Sum(x => GetDiffComplexityOfNew(context, a1[x]));
            var movedOrDiffedItemComplexity = permutation.Sum(x => GetDiffComplexity(context, a1[x.First], a2[x.Second]));
            return addedIndexComplexity + removedIndexComplexity + movedOrDiffedItemComplexity;
        }

        private static bool AreEqual(Context context, object v1, object v2)
        {
            if (v1 is IDictionary<string, object> o1 && v2 is IDictionary<string, object> o2)
                return context.EqualityCache.GetOrAdd(Tuples.SameTypePair(v1, v2), 
                    x => o1.Keys.Count == o2.Count && o1.Keys.All(y => AreEqual(context, o1[y], o2[y])));
            if (v1 is IList<object> a1 && v2 is IList<object> a2)
                return context.EqualityCache.GetOrAdd(Tuples.SameTypePair(v1, v2), 
                    x => a1.Count == a2.Count && Enumerable.Range(0, a1.Count).All(y => AreEqual(context, a1[y], a2[y])));
            if (v1 is string s1 && v2 is string s2)
                return s1 == s2;
            if (v1 is double d1 && v2 is double d2)
                return d1 == d2;
            if (v1 is int i1 && v2 is int i2)
                return i1 == i2;
            if (v1 is bool b1 && v2 is bool b2)
                return b1 == b2;
            if (v1 == null && v2 == null)
                return true;
            return false;
        }

        private static int GetDiffComplexityOfNew(Context context, object val)
        {
            if (val == null)
                return 1;
            return context.SingleComplexityCache.GetOrAdd(val, x => CalcDiffComplexityOfNew(context, x));
        }

        private static int CalcDiffComplexityOfNew(Context context, object val)
        {
            switch (val)
            {
                case IDictionary<string, object> o:
                    return o.Values.Sum(x => GetDiffComplexityOfNew(context, x)) + 1;
                case IList<object> l:
                    return l.Sum(x => GetDiffComplexityOfNew(context, x)) + 1;
                default:
                    return 1;
            }
        }

        private static IReadOnlyList<Pair<int>> BuildArrayPermutation(Context context, IList<object> a1, IList<object> a2)
        {
            var swaps = new List<Pair<int>>();
            for (var i = 0; i < a1.Count; i++)
            {
                var iv = a1[i];
                switch (iv) 
                {
                    case IDictionary<string, object> io:
                    {
                        var bestCondidate = Enumerable.Range(0, a2.Count)
                            .Where(x => !swaps.Any(p => p.Second == x))
                            .Select(x => Tuples.Pair(x, a2[x] is IDictionary<string, object> jo ? jo : null))
                            .Where(x => x.Second != null && AreSameObject(context, io, x.Second))
                            .MinimalOrNull(x => GetDiffComplexityOfObject(context, io, x.Second));
                        if (bestCondidate.HasValue)
                            swaps.Add(Tuples.SameTypePair(i, bestCondidate.Value.First));
                        break;
                    }
                    case IList<object> ia:
                    {
                        var bestCondidate = Enumerable.Range(0, a2.Count)
                            .Where(x => !swaps.Any(p => p.Second == x))
                            .Select(x => Tuples.Pair(x, a2[x] is IList<object> ja ? ja : null))
                            .Where(x => x.Second != null)
                            .MinimalOrNull(x => GetDiffComplexityOfArray(context, ia, x.Second));
                        if (bestCondidate.HasValue)
                            swaps.Add(Tuples.SameTypePair(i, bestCondidate.Value.First));
                        break;
                    }
                    default:
                    {
                        var bestCondidate = Enumerable.Range(0, a2.Count)
                            .Where(x => !swaps.Any(p => p.Second == x))
                            .Where(x => AreEqual(context, iv, a2[x]))
                            .FirstOrNull();
                        if (bestCondidate.HasValue)
                            swaps.Add(Tuples.SameTypePair(i, bestCondidate.Value));
                        break;
                    }
                }
            }
            return swaps;
        }

        private static bool AreSameObject(Context context, IDictionary<string, object> o1, IDictionary<string, object> o2)
        {
            return AreEqual(context, o1, o2) || context.IdentityComparer.AreSameObject(o1, o2);
        }
    }
}