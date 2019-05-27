using System;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwSerializationHandler
    {
        Type Type { get; }
        bool ContentIsProperties { get; }
        void SaveContent(ITrwSerializationWriteContext context, object value);
        object LoadContent(ITrwSerializationReadContext context);
        void ApplyDiff(ITrwSerializationDiffApplier applier, object target, ITrwDiff diff, TrwDiffDirection direction);
    }

    public interface ITrwSerializationHandler<T> : ITrwSerializationHandler
    {
        void SaveContent(ITrwSerializationWriteContext context, T value);
        new T LoadContent(ITrwSerializationReadContext context);
        void ApplyDiff(ITrwSerializationDiffApplier applier, T target, ITrwDiff diff, TrwDiffDirection direction);
    }
}