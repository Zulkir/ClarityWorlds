using System;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public abstract class TrwSerializationHandlerBase<T> : ITrwSerializationHandler<T>
    {
        public Type Type => typeof(T);

        void ITrwSerializationHandler.SaveContent(ITrwSerializationWriteContext context, object value) => SaveContent(context, (T)value);
        object ITrwSerializationHandler.LoadContent(ITrwSerializationReadContext context) => LoadContent(context);
        public void ApplyDiff(ITrwSerializationDiffApplier applier, object target, ITrwDiff diff, TrwDiffDirection direction) => ApplyDiff(applier, (T)target, diff, direction);

        public abstract bool ContentIsProperties { get; }
        public abstract void SaveContent(ITrwSerializationWriteContext context, T value);
        public abstract T LoadContent(ITrwSerializationReadContext context);
        public virtual void ApplyDiff(ITrwSerializationDiffApplier applier, T target, ITrwDiff diff, TrwDiffDirection direction) => 
            throw new NotSupportedException($"TrwHandler '{GetType().Name}' does not support diffs.");
    }
}