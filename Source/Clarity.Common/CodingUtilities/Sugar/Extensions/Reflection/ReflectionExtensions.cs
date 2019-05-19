using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Reflection
{
    public static class ReflectionExtensions
    {
        public static Type[] GetAllInterfaces(this Type baseInterface) => 
            EnumerateAllInterfacesWithRepetitions(baseInterface).Distinct().ToArray();

        private static IEnumerable<Type> EnumerateAllInterfacesWithRepetitions(Type baseInterface)
        {
            yield return baseInterface;
            foreach (var subInterface in baseInterface.GetInterfaces())
                foreach (var subsubinterface in EnumerateAllInterfacesWithRepetitions(subInterface))
                    yield return subsubinterface;
        }

        public static MethodInfo[] GetAllMethods(this Type type) => 
            type.GetAllInterfaces().SelectMany(x => x.GetMethods()).ToArray();

        public static PropertyInfo[] GetAllProperties(this Type type) =>
            type.GetAllInterfaces().SelectMany(x => x.GetProperties()).ToArray();
    }
}