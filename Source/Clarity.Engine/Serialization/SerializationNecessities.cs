using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Engine.Serialization
{
    public class SerializationNecessities : ISerializationNecessities
    {
        private readonly Dictionary<string, ITrwSerializationHandlerContainer> containers;
        private readonly Dictionary<string, IReadOnlyList<ITrwSerializationTypeRedirect>> typeRedirects;

        public SerializationNecessities(IReadOnlyList<ISerializationNecessitiesProvider> providers)
        {
            containers = new Dictionary<string, ITrwSerializationHandlerContainer>();
            typeRedirects = new Dictionary<string, IReadOnlyList<ITrwSerializationTypeRedirect>>();

            var allSerializationTypes = providers.SelectMany(x => x.SerializationTypes).Distinct().ToArray();
            foreach (var serializationType in allSerializationTypes)
            {
                var familiesForType = providers
                    .Where(x => x.SerializationTypes.Contains(serializationType))
                    .SelectMany(x => x.Families)
                    .ToArray();
                var container = new TrwSerializationHandlerContainer(familiesForType);
                containers.Add(serializationType, container);

                var typeRedirectsForType = providers
                    .Where(x => x.SerializationTypes.Contains(serializationType))
                    .SelectMany(x => x.TypeRedirects)
                    .ToArray();
                typeRedirects.Add(serializationType, typeRedirectsForType);
            }
        }

        public ITrwSerializationHandlerContainer GetTrwHandlerContainer(string serializationType) => 
            containers[serializationType];

        public IReadOnlyList<ITrwSerializationTypeRedirect> GetTrwHandlerTypeRedirects(string serializationType) => 
            typeRedirects[serializationType];
    }
}