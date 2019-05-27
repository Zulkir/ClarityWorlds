using System;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwSerializationDiffApplier
    {
        void ApplyDiff<T>(T target, ITrwDiff diff, TrwDiffDirection direction);
        object ToDynamic(object value);
        object FromDynamic(Type type, object dynValue);
    }
}
