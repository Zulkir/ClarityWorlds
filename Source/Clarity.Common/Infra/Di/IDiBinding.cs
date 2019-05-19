using System;

namespace Clarity.Common.Infra.Di
{
    public interface IDiBinding
    {
        Type ConcreteType { get; }
        object BuildInstance(IDiContainer di, Type[] genericArguments, DiBuildInstanceType buildInstanceType);
    }
}