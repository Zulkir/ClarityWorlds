using System;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    public class PropertyBagTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (typeof(IPropertyBag).IsAssignableFrom(type))
            {
                handler = new PropertyBagTrwHandler();
                return true;
            }

            handler = null;
            return false;
        }
    }
}