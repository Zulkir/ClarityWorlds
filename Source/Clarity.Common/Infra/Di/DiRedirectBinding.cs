using System;

namespace Clarity.Common.Infra.Di
{
    public class DiRedirectBinding : IDiBinding
    {
        private readonly Type concreteType;

        public DiRedirectBinding(Type concreteType)
        {
            this.concreteType = concreteType;
        }

        public Type ConcreteType { get { return concreteType; } }

        public object BuildInstance(IDiContainer di, Type[] genericArguments, DiBuildInstanceType buildInstanceType)
        {
            var resolvedType = concreteType.IsGenericType 
                ? concreteType.MakeGenericType(genericArguments) 
                : concreteType;
            switch (buildInstanceType)
            {
                case DiBuildInstanceType.Singleton: return di.Get(resolvedType);
                case DiBuildInstanceType.NewEachTime: return di.Instantiate(resolvedType);
                default: throw new ArgumentOutOfRangeException(nameof(buildInstanceType), buildInstanceType, null);
            }
        }
    }
}