using System;
using System.Linq;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding.Diffs;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public abstract class ObjectDiffableTrwHandlerBase<TObj, TValue, TProp> : ObjectTrwHandlerBase<TObj, TObj, TValue, TProp>
    {
        protected override TObj Finalize(TObj builder) => builder;

        public override void ApplyDiff(ITrwSerializationDiffApplier applier, TObj target, ITrwDiff diff, TrwDiffDirection direction)
        {
            if (typeof(TObj).IsValueType)
            {
                base.ApplyDiff(applier, target, diff, direction);
                return;
            }

            if (!(diff is MutateObjectTrwDiff odiff))
                throw new ArgumentException("Diff of type MutateObjectTrwDiff is expected.");
            if (odiff.AddedProperties.Any() || odiff.RemovedProperties.Any())
                throw new ArgumentException("Diffs with added or removed properties are not applicable.");
            foreach (var kvp in odiff.DiffedProperties)
            {
                if (!TryGetProp(target, kvp.First, out var prop))
                    continue;
                var forward = direction == TrwDiffDirection.Forward;
                switch (kvp.Second)
                {
                    case ReplaceValueTrwDiff replaceDiff:
                        var newValue = (TValue)applier.FromDynamic(GetPropType(prop), forward ? replaceDiff.NewValue : replaceDiff.OldValue);
                        SetPropValue(target, target, prop, newValue);
                        break;
                    default:
                        var value = GetPropValue(target, prop);
                        applier.ApplyDiff(value, kvp.Second, direction);
                        break;
                }
            }
        }
    }
}