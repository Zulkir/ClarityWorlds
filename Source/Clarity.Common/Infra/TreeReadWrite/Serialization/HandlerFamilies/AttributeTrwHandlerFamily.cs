using System;
using System.Diagnostics;
using System.Reflection;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    public class AttributeTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        private readonly ITrwAttributeObjectCreator objectCreator;

        public AttributeTrwHandlerFamily(ITrwAttributeObjectCreator objectCreator)
        {
            this.objectCreator = objectCreator;
        }

        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (type.GetCustomAttribute<TrwSerializeAttribute>() != null)
            {
                var constructor = typeof(AttributeTrwHandler<>).MakeGenericType(type).GetConstructor(new []{typeof(ITrwAttributeObjectCreator)});
                Debug.Assert(constructor != null, "constructor != null");
                handler = (ITrwSerializationHandler)constructor.Invoke(new object[]{ objectCreator });
                return true;
            }

            handler = null;
            return false;
        }
    }
}