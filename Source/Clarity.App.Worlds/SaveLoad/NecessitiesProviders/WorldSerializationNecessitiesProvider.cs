using System.Collections.Generic;
using Clarity.App.Worlds.SaveLoad.TrwExtensions;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Serialization;

namespace Clarity.App.Worlds.SaveLoad.NecessitiesProviders
{
    public class WorldSerializationNecessitiesProvider : ISerializationNecessitiesProvider
    {
        public IReadOnlyList<string> SerializationTypes { get; } = new[]
        {
            SaveLoadConstants.WorldSerializationType
        };

        public IReadOnlyList<ITrwSerializationHandlerFamily> Families { get; } = new ITrwSerializationHandlerFamily[]
        {
            new ResourceTrwHandlerFamily(),
            new AssetTrwHandlerFamily(),
            new GeneratedResourceSourceTrwHandlerFamily()
        };
        public IReadOnlyList<ITrwSerializationTypeRedirect> TypeRedirects { get; } = new ITrwSerializationTypeRedirect[]
        {
            new ResourceTrwTypeRedirect(),
            new AssetForWorldTrwTypeRedirect(),
        };
    }
}