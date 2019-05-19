using System;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public interface IAmObjectClassFactory
    {
        IAmObjectInstantiator CreateObjectClass(Type objectInterface, Func<Type, object> getDefaultDependency);
    }
}