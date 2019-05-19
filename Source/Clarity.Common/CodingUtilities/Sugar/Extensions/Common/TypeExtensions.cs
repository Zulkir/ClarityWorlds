using System;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Common
{
    public static class TypeExtensions
    {
        public static bool IsGenericlyAssignableFrom(this Type abstractType, Type concreteType)
        {
            if (abstractType.IsGenericTypeDefinition)
            {
                var typeArgs = Enumerable.Range(0, concreteType.GetGenericArguments().Length).Select(x => typeof(int)).ToArray();
                return abstractType.MakeGenericType(typeArgs).IsAssignableFrom(concreteType.MakeGenericType(typeArgs));
            }
            return abstractType.IsAssignableFrom(concreteType);
        }

        public static bool IsNullable(this Type type) => 
            type.IsConstructedGenericType && typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition());

        public static Type DeepSubstituteGenerics(this Type type, IReadOnlyDictionary<string, Type> genericArgumentMap)
        {
            if (type.IsGenericParameter)
                return genericArgumentMap[type.Name];
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition().MakeGenericType(type.GetGenericArguments().Select(x => DeepSubstituteGenerics(x, genericArgumentMap)).ToArray());
            if (type.HasElementType)
                return DeepSubstituteGenerics(type.GetElementType(), genericArgumentMap).MakeArrayType();
            return type;
        }
    }
}