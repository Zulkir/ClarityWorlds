using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Common.Infra.TreeReadWrite.DiffBuilding.Diffs
{
    public class MutateObjectTrwDiff : ITrwDiff
    {
        public IReadOnlyList<Pair<string, object>> AddedProperties { get; }
        public IReadOnlyList<Pair<string, object>> RemovedProperties { get; }
        public IReadOnlyList<Pair<string, ITrwDiff>> DiffedProperties { get; }

        public bool IsEmpty => AddedProperties.IsEmpty() && RemovedProperties.IsEmpty() &&
                               DiffedProperties.All(x => x.Second.IsEmpty);

        public MutateObjectTrwDiff(IReadOnlyList<Pair<string, object>> addedProperties, 
                                   IReadOnlyList<Pair<string, object>> removedProperties, 
                                   IReadOnlyList<Pair<string, ITrwDiff>> diffedProperties)
        {
            AddedProperties = addedProperties;
            RemovedProperties = removedProperties;
            DiffedProperties = diffedProperties;
        }
    }
}