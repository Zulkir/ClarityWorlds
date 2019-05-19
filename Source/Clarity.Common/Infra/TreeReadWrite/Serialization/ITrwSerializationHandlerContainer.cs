using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwSerializationHandlerContainer
    {
        ITrwSerializationHandler GetHandler(Type type);
        ITrwSerializationHandler<T> GetHandler<T>();
    }
}