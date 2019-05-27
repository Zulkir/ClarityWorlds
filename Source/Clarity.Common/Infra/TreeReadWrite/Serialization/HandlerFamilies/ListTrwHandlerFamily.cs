using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Reflection;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies 
{
    public class ListTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            var listInterface = type.GetAllInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
            if (listInterface == null)
            {
                handler = null;
                return false;
            }

            var itemType = listInterface.GenericTypeArguments[0];
            var ctor = typeof(ListTrwHandler<>).MakeGenericType(itemType).GetConstructor(Type.EmptyTypes);
            Debug.Assert(ctor != null, nameof(ctor) + " != null");
            handler = (ITrwSerializationHandler)ctor.Invoke(EmptyArrays<object>.Array);
            return true;
        }
    }
}