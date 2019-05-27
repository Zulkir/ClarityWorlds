using System;
using System.Diagnostics;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions
{
    public class AmObjectSerializationHandlerFamily : ITrwSerializationHandlerFamily
    {
        private readonly IAmDiBasedObjectFactory objectFactory;

        public AmObjectSerializationHandlerFamily(IAmDiBasedObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (typeof(IAmObject).IsAssignableFrom(type))
            {
                var handlerCtor =
                        typeof(AmObjectTrwHandler<>).MakeGenericType(type)
                            .GetConstructor(new[] { typeof(IAmDiBasedObjectFactory) });
                Debug.Assert(handlerCtor != null, "handlerCtor != null");
                handler = (ITrwSerializationHandler)handlerCtor.Invoke(new object[] { objectFactory });
                return true;
            }

            handler = null;
            return false;
        }
    }
}