using System.Collections.Generic;
using Clarity.App.Worlds.SaveLoad.TrwExtensions;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Serialization;

namespace Clarity.App.Worlds.SaveLoad.NecessitiesProviders
{
    public class AmSerializationNecessitiesProvider : ISerializationNecessitiesProvider
    {
        public IReadOnlyList<string> SerializationTypes { get; }
        public IReadOnlyList<ITrwSerializationHandlerFamily> Families { get; }
        public IReadOnlyList<ITrwSerializationTypeRedirect> TypeRedirects { get; }

        public AmSerializationNecessitiesProvider(IAmDiBasedObjectFactory objectFactory)
        {
            SerializationTypes = new[]
            {
                SaveLoadConstants.BasicSerializationType,
                SaveLoadConstants.WorldSerializationType
            };
            Families = new ITrwSerializationHandlerFamily[]
            {
                new AmObjectSerializationHandlerFamily(objectFactory),
            };
            TypeRedirects = new ITrwSerializationTypeRedirect[]
            {
                new AmTrwTypeRedirect(),
            };
        }
    }
}