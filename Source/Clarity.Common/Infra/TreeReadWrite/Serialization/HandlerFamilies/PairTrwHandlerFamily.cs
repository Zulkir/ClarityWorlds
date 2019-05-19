using System;
using System.Diagnostics;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    // todo: to Tuple handler family
    public class PairTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (!type.IsGenericType)
            {
                handler = null;
                return false;
            }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            if (genericTypeDefinition == typeof(Pair<>))
            {
                var actualType = type.GetGenericArguments().Single();
                var constructor = typeof(PairTrwHandler<>).MakeGenericType(actualType).GetConstructor(Type.EmptyTypes);
                Debug.Assert(constructor != null, "constructor != null");
                handler = (ITrwSerializationHandler)constructor.Invoke(EmptyArrays<object>.Array);
                return true;
            }

            // todo: Pair<T1, T2>
            // todo: UnorderedPair

            handler = null;
            return false;
        }
    }
}