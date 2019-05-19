using System;
using System.Text;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    public class CommonTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            handler = CreateHandlerOrNull(type);
            return handler != null;
        }

        private static ITrwSerializationHandler CreateHandlerOrNull(Type type)
        {
            if (type.IsEnum)
                return (ITrwSerializationHandler)Activator.CreateInstance(typeof(EnumTrwHandler<>).MakeGenericType(type));
            if (typeof(Type).IsAssignableFrom(type))
                return new TypeTrwHandler();
            if (type == typeof(StringBuilder))
                return new ProxyTrwHandler<StringBuilder, string>(x => x.ToString(), x => new StringBuilder(x), false);
            if (type == typeof(Guid))
                return new GuidTrwHandler();
            return null;
        }
    }
}