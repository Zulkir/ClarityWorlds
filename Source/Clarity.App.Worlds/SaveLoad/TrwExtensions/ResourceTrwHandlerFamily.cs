using System;
using System.Diagnostics;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions
{
    public class ResourceTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (typeof(IResource).IsAssignableFrom(type))
            {
                var handlerCtor = typeof(ResourceTrwHandler<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes);
                Debug.Assert(handlerCtor != null, nameof(handlerCtor) + " != null");
                handler = (ITrwSerializationHandler)handlerCtor.Invoke(EmptyArrays<object>.Array);
                return true;
            }
            handler = null;
            return false;
        }
    }
}