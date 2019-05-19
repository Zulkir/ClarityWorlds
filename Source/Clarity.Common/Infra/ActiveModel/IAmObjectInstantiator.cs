using System;

namespace Clarity.Common.Infra.ActiveModel
{
    public interface IAmObjectInstantiator
    {
        Type Class { get; }
        IAmObject Instantiate(Func<Type, object> getArgument = null);
    }

    public interface IAmObjectInstantiator<out T> : IAmObjectInstantiator
        where T : IAmObject
    {
        new T Instantiate(Func<Type, object> getArgument = null);
    }
}