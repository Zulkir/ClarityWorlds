using System;
using System.Diagnostics;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    public class ArrayTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (!type.IsArray)
            {
                handler = null;
                return false;
            }

            var constructor = typeof(ArrayTrwHandler<>).MakeGenericType(type.GetElementType()).GetConstructor(Type.EmptyTypes);
            Debug.Assert(constructor != null, nameof(constructor) + " != null");
            handler = (ITrwSerializationHandler)constructor.Invoke(new object[0]);
            return true;
        }
    }
}