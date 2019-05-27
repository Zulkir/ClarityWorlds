using System;

namespace Clarity.Common.Infra.DependencyInjection
{
    public class DiObjectBinding : IDiBinding
    {
        private readonly object obj;

        public DiObjectBinding(object obj)
        {
            this.obj = obj;
        }

        public Type ConcreteType { get { return obj.GetType(); } }

        public object BuildInstance(IDiContainer di, Type[] genericArguments, DiBuildInstanceType buildInstanceType)
        {
            return obj;
        }
    }
}