using System;
using System.Diagnostics;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    public class NullableTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (!type.IsNullable())
            {
                handler = null;
                return false;
            }

            var actualType = type.GetGenericArguments().Single();
            var contentIsProps = container.GetHandler(actualType).ContentIsProperties;
            var constructor = typeof(NullableTrwHandler<>).MakeGenericType(actualType).GetConstructor(new [] {typeof(bool)});
            Debug.Assert(constructor != null, "constructor != null");
            handler = (ITrwSerializationHandler)constructor.Invoke(new object[]{ contentIsProps });
            return true;
        }
    }
}