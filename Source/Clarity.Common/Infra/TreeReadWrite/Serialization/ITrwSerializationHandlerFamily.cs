using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwSerializationHandlerFamily
    {
        bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler);
    }
}