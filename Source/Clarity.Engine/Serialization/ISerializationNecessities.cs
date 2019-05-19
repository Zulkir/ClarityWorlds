using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Engine.Serialization
{
    public interface ISerializationNecessities
    {
        ITrwSerializationHandlerContainer GetTrwHandlerContainer(string serializationType);
        IReadOnlyList<ITrwSerializationTypeRedirect> GetTrwHandlerTypeRedirects(string serializationType);
    }
}