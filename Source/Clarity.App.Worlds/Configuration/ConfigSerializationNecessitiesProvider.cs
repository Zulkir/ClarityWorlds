using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies;
using Clarity.Common.Infra.TreeReadWrite.Serialization.TypeRedirects;
using Clarity.Engine.Serialization;

namespace Clarity.App.Worlds.Configuration
{
    public class ConfigSerializationNecessitiesProvider : ISerializationNecessitiesProvider
    {
        public static string SerializationType { get; } = "pres_config";

        public IReadOnlyList<string> SerializationTypes { get; }
        public IReadOnlyList<ITrwSerializationHandlerFamily> Families { get; }
        public IReadOnlyList<ITrwSerializationTypeRedirect> TypeRedirects { get; }

        public ConfigSerializationNecessitiesProvider(ITrwAttributeObjectCreator objectCreator)
        {
            SerializationTypes = new[]
            {
                SerializationType,
            };

            Families = new ITrwSerializationHandlerFamily[]
            {
                new ArrayTrwHandlerFamily(),
                new ListTrwHandlerFamily(),
                new AttributeTrwHandlerFamily(objectCreator),
                new BasicTrwHandlerFamily(),
                new CommonTrwHandlerFamily(),
                new NumericalsTrwHandlerFamily(),
                new NullableTrwHandlerFamily(),
                new PairTrwHandlerFamily(),
                new StringDictionaryTrwHandlerFamily(),
                new PropertyBagTrwHandlerFamily(),
            };

            TypeRedirects = new ITrwSerializationTypeRedirect[]
            {
                new RuntimeTypeTrwTypeRedirect(),
            };
        }
    }
}