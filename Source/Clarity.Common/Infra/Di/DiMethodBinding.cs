using System;
using System.Linq;
using System.Reflection;

namespace Clarity.Common.Infra.Di
{
    public class DiMethodBinding : IDiBinding
    {
        private readonly object obj;
        private readonly MethodInfo method;

        public DiMethodBinding(object obj, MethodInfo method)
        {
            this.obj = obj;
            this.method = method;
        }

        public Type ConcreteType { get { return method.DeclaringType; } }

        public object BuildInstance(IDiContainer di, Type[] genericArguments, DiBuildInstanceType buildInstanceType)
        {
            var resolvedMethod = method.IsGenericMethod ? method.MakeGenericMethod(genericArguments) : method;
            var parameters = resolvedMethod.GetParameters().Select(x => di.Get(x.ParameterType)).ToArray();
            return resolvedMethod.Invoke(obj, parameters);
        }
    }
}