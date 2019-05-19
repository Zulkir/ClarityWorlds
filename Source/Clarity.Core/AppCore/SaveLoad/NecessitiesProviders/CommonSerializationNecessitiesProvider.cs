using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies;
using Clarity.Engine.Serialization;

namespace Clarity.Core.AppCore.SaveLoad.NecessitiesProviders
{
    public class CommonSerializationNecessitiesProvider : ISerializationNecessitiesProvider
    {
        public IReadOnlyList<string> SerializationTypes { get; }
        public IReadOnlyList<ITrwSerializationHandlerFamily> Families { get; }
        public IReadOnlyList<ITrwSerializationTypeRedirect> TypeRedirects { get; }

        public CommonSerializationNecessitiesProvider(ITrwAttributeObjectCreator objectCreator)
        {
            SerializationTypes = new[]
            {
                SaveLoadConstants.BasicSerializationType,
                SaveLoadConstants.WorldSerializationType
            };

            Families = new ITrwSerializationHandlerFamily[]
            {
                new ArrayTrwHandlerFamily(),
                new AttributeTrwHandlerFamily(objectCreator),
                new BasicTrwHandlerFamily(),
                new CommonTrwHandlerFamily(),
                new NumericalsTrwHandlerFamily(),
                new NullableTrwHandlerFamily(),
                new PairTrwHandlerFamily(),
                new StringDictionaryTrwHandlerFamily(),
            };

            TypeRedirects = new ITrwSerializationTypeRedirect[]
            {
                new RuntimeTypeTrwTypeRedirect(),
            };
        }
    }
}