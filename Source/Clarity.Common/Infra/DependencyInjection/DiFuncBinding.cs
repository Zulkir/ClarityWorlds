using System;

namespace Clarity.Common.Infra.DependencyInjection
{
    public class DiFuncBinding : IDiBinding
    {
        private readonly Func<IDiContainer, object> create;
        private readonly Type concreteType;

        public DiFuncBinding(Func<IDiContainer, object> create, Type concreteType)
        {
            this.create = create;
            this.concreteType = concreteType;
        }

        public Type ConcreteType { get { return concreteType; } }

        public object BuildInstance(IDiContainer di, Type[] genericArguments, DiBuildInstanceType buildInstanceType)
        {
            return create(di);
        }
    }
}