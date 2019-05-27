using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public class TrwSerializationHandlerContainer : ITrwSerializationHandlerContainer
    {
        private readonly ITrwSerializationHandlerFamily[] families;
        private readonly Dictionary<Type, ITrwSerializationHandler> handlers;
        
        public TrwSerializationHandlerContainer(IReadOnlyList<ITrwSerializationHandlerFamily> handlerFamilies)
        {
            families = handlerFamilies.ToArray();
            handlers = new Dictionary<Type, ITrwSerializationHandler>();
        }

        public ITrwSerializationHandler<T> GetHandler<T>() => 
            (ITrwSerializationHandler<T>)GetHandler(typeof(T));

        public ITrwSerializationHandler GetHandler(Type type) => 
            handlers.GetOrAdd(type, CreateHandler);

        private ITrwSerializationHandler CreateHandler(Type type)
        {
            ITrwSerializationHandler handler = null;
            return families.Any(family => family.TryCreateHandlerFor(type, this, out handler))
                ? handler
                : throw new ArgumentException($"No serialization handler found for the type '{type}'.");
        }
    }
}