using System;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding.Diffs;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers 
{
    public abstract class ArrayDiffableTrwHandlerBase<TArray, TItem> : ArrayTrwHandlerBase<TArray, TArray, TItem>
    {
        protected abstract int GetCount(TArray array);
        protected abstract TItem GetItem(TArray array, int index);
        protected abstract void RemoveAt(TArray array, int index);
        protected abstract void Insert(TArray array, int index, TItem value);

        protected override TArray Finalize(TArray builder) => builder;

        public override void ApplyDiff(ITrwSerializationDiffApplier applier, TArray target, ITrwDiff diff, TrwDiffDirection direction)
        {
            if (!(diff is MutateArrayTrwDiff adiff))
                throw new ArgumentException("Diff of type MutateArrayTrwDiff expected.");
            TItem[] result;
            switch (direction)
            {
                case TrwDiffDirection.Forward:
                {
                    result = new TItem[GetCount(target) + adiff.AddedItems.Count - adiff.RemovedItems.Count];
                    foreach (var pair in adiff.AddedItems)
                        result[pair.First] = (TItem)applier.FromDynamic(typeof(TItem), pair.Second);
                    foreach (var pair in adiff.MovedItems)
                        result[pair.Second] = GetItem(target, pair.First);
                    foreach (var pair in adiff.DiffedItems)
                    {
                        var item = GetItem(target, pair.First.First);
                        applier.ApplyDiff(item, pair.Second, direction);
                        result[pair.First.Second] = item;
                    }
                    break;
                }
                case TrwDiffDirection.Backward:
                {
                    result = new TItem[GetCount(target) - adiff.AddedItems.Count + adiff.RemovedItems.Count];
                    foreach (var pair in adiff.RemovedItems)
                        result[pair.First] = (TItem)applier.FromDynamic(typeof(TItem), pair.Second);
                    foreach (var pair in adiff.MovedItems)
                        result[pair.First] = GetItem(target, pair.Second);
                    foreach (var pair in adiff.DiffedItems)
                    {
                        var item = GetItem(target, pair.First.Second);
                        applier.ApplyDiff(item, pair.Second, direction);
                        result[pair.First.First] = item;
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            foreach (var i in adiff.UnaffectedItems)
                result[i] = GetItem(target, i);
            for (var i = 0; i < result.Length; i++)
            {
                var item = result[i];
                var targetIndex = 
                    typeof(TItem).IsValueType 
                        ? typeof(IEquatable<TItem>).IsAssignableFrom(typeof(TItem)) 
                            ? EnumerateItems(target).IndexOf(x => ((IEquatable<TItem>)x).Equals(item))
                            : EnumerateItems(target).IndexOf(x => Equals(x, item)) 
                        : EnumerateItems(target).IndexOf(x => ReferenceEquals(x, item));
                if (targetIndex.HasValue)
                {
                    if (targetIndex.Value == i) 
                        continue;
                    RemoveAt(target, targetIndex.Value);
                    Insert(target, i, item);
                }
                else
                {
                    Insert(target, i, item);
                }
            }
            for (var i = GetCount(target) - 1; i >= result.Length; i--)
            {
                RemoveAt(target, i);
            }
        }
    }
}