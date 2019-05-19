using System;

namespace Clarity.Common.Infra.ActiveModel
{
    public interface IAmDiBasedObjectFactory
    {
        TObj Create<TObj>() where TObj : IAmObject;
        bool HasInstantiator(Type objectOriginType);
        IAmObjectInstantiator<TObj> GetInstantiator<TObj>() where TObj : IAmObject;
        IAmObjectInstantiator GetInstantiator(Type objectType);
    }
}