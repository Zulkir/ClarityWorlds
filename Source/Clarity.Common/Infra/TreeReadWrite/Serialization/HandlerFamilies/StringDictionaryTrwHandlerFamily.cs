using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    public class StringDictionaryTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            // todo: separate to IDictionary and IEnumerable<KVP>

            handler = null;

            var enumType = type.GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumType == null)
                return false;

            var enumArgType = enumType.GetGenericArguments().Single();

            if (!enumArgType.IsGenericType || enumArgType.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
                return false;

            var pairTypeGenericArguments = enumArgType.GetGenericArguments();
            var keyType = pairTypeGenericArguments[0];
            var valueType = pairTypeGenericArguments[1];
            if (keyType != typeof(string))
                return false;

            if (!type.IsAssignableFrom(typeof(Dictionary<,>).MakeGenericType(keyType, valueType)))
                return false;
            
            var ctor = typeof(StringDictionaryTrwHandler<,>).MakeGenericType(type, valueType).GetConstructor(Type.EmptyTypes);
            Debug.Assert(ctor != null, nameof(ctor) + " != null");
            handler = (ITrwSerializationHandler)ctor.Invoke(EmptyArrays<object>.Array);
            Debug.Assert(handler.Type == type, "handler.Type == type");
            return true;
        }
    }
}