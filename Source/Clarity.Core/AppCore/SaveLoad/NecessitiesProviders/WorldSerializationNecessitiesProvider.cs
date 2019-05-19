using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Serialization;

namespace Clarity.Core.AppCore.SaveLoad.NecessitiesProviders
{
    public class WorldSerializationNecessitiesProvider : ISerializationNecessitiesProvider
    {
        public IReadOnlyList<string> SerializationTypes { get; } = new[] {SaveLoadConstants.WorldSerializationType};

        public IReadOnlyList<ITrwSerializationHandlerFamily> Families { get; } = new ITrwSerializationHandlerFamily[]
        {
            new ResourceTrwHandlerFamily(),
            new AssetTrwHandlerFamily(),
        };
        public IReadOnlyList<ITrwSerializationTypeRedirect> TypeRedirects { get; } = new ITrwSerializationTypeRedirect[]
        {
            new ResourceTrwTypeRedirect(),
            new AssetForWorldTrwTypeRedirect(),
        };
    }
}