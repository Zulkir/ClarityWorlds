using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Clarity.Common.Infra.Di
{
    public class DiConstructorBinding : IDiBinding
    {
        private readonly ConstructorInfo constructor;

        public DiConstructorBinding(ConstructorInfo constructor)
        {
            this.constructor = constructor;
        }

        public DiConstructorBinding(Type type)
        {
            string errorMessage;
            if (!TryGetSingleUsableConstructor(type, out constructor, out errorMessage))
                throw new ArgumentException(errorMessage);
        }

        public Type ConcreteType { get { return constructor.DeclaringType; } }

        public object BuildInstance(IDiContainer di, Type[] genericArguments, DiBuildInstanceType buildInstanceType)
        {
            var resolvedCtor = ResolveGenerics(constructor, genericArguments);
            var parameters = resolvedCtor.GetParameters().Select(x => di.Get(x.ParameterType)).ToArray();
            return resolvedCtor.Invoke(parameters);
        }

        private static ConstructorInfo ResolveGenerics(ConstructorInfo constructor, Type[] genericArguments)
        {
            if (!constructor.ContainsGenericParameters)
                return constructor;
            var realParamTypes = constructor.GetParameters()
                .Select(x => x.ParameterType)
                .Select(x => x.IsGenericParameter ? null : x)
                .ToArray();

            int j = 0;
            for (int i = 0; i < realParamTypes.Length; i++)
                if (realParamTypes[i] == null)
                {
                    realParamTypes[i] = genericArguments[j];
                    j++;
                }

            Debug.Assert(constructor.DeclaringType != null, "constructor.DeclaringType != null");
            return constructor.DeclaringType.MakeGenericType(genericArguments).GetConstructor(realParamTypes);
        }

        private static bool IsConstructorUsable(ConstructorInfo info)
        {
            return info.IsPublic;
        }

        public static bool TryCreate(Type type, out DiConstructorBinding binding)
        {
            ConstructorInfo constructor;
            string errorMessage;
            if (!TryGetSingleUsableConstructor(type, out constructor, out errorMessage))
            {
                binding = null;
                return false;
            }
            binding = new DiConstructorBinding(constructor);
            return true;
        }

        private static bool TryGetSingleUsableConstructor(Type type, out ConstructorInfo constructor, out string errorMessage)
        {
            var constructors = type.GetTypeInfo().DeclaredConstructors
                .Where(IsConstructorUsable)
                .ToArray();
            if (constructors.Length == 0)
            {
                errorMessage = $"Implementation type '{type.FullName}' has no constructors for DiContainer to use.";
                constructor = null;
                return false;
            }
            if (constructors.Length > 1)
            {
                errorMessage = $"Implementation type '{type.FullName}' has more than one constructor for DiContainer to choose from.";
                constructor = null;
                return false;
            }
            constructor = constructors[0];
            errorMessage = null;
            return true;
        }
    }
}