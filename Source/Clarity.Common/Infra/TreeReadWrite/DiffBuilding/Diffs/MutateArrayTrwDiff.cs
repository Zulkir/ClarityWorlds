using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Common.Infra.TreeReadWrite.DiffBuilding.Diffs
{
    public class MutateArrayTrwDiff : ITrwDiff
    {
        public IReadOnlyList<Pair<int, object>> AddedItems { get; }
        public IReadOnlyList<Pair<int, object>> RemovedItems { get; }
        public IReadOnlyList<Pair<int>> MovedItems { get; }
        public IReadOnlyList<Pair<Pair<int>, ITrwDiff>> DiffedItems { get; }
        public IReadOnlyList<int> UnaffectedItems { get; }

        public bool IsEmpty => AddedItems.IsEmpty() && RemovedItems.IsEmpty() && MovedItems.IsEmpty() &&
                               DiffedItems.All(x => x.Second.IsEmpty);

        public MutateArrayTrwDiff(IReadOnlyList<Pair<int, object>> addedItems, 
                               IReadOnlyList<Pair<int, object>> removedItems, 
                               IReadOnlyList<Pair<int>> movedItems, 
                               IReadOnlyList<Pair<Pair<int>, ITrwDiff>> diffedItems, 
                               IReadOnlyList<int> unaffectedItems)
        {
            AddedItems = addedItems;
            RemovedItems = removedItems;
            MovedItems = movedItems;
            DiffedItems = diffedItems;
            UnaffectedItems = unaffectedItems;
        }
    }
}