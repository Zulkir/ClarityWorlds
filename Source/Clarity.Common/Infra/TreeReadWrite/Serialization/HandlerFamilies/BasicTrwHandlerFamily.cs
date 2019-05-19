using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    public class BasicTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            handler = CreateHandlerOrNull(type);
            return handler != null;
        }

        private static ITrwSerializationHandler CreateHandlerOrNull(Type type)
        {
            if (type == typeof(bool))
                return new BoolTrwHandler();
            if (type == typeof(int))
                return new IntTrwHandler();
            if (type == typeof(float))
                return new FloatTrwHandler();
            if (type == typeof(string))
                return new StringTrwHandler();
            return null;
        }
    }
}