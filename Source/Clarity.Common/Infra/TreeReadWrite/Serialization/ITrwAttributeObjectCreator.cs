using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwAttributeObjectCreator
    {
        Func<T> GetConstructor<T>();
    }
}