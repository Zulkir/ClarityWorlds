using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Engine.Serialization
{
    public interface ISerializationNecessitiesProvider
    {
        IReadOnlyList<string> SerializationTypes { get; }
        IReadOnlyList<ITrwSerializationHandlerFamily> Families { get; }
        IReadOnlyList<ITrwSerializationTypeRedirect> TypeRedirects { get; }
    }
}