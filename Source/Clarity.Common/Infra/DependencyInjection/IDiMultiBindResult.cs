using System;

namespace Clarity.Common.Infra.DependencyInjection
{
    public interface IDiMultiBindResult
    {
        IDiMultiToResult To<TConcrete>(TConcrete obj) where TConcrete : class;
        IDiMultiToResult To<TConcrete>(Func<IDiContainer, TConcrete> create) where TConcrete : class;
        IDiMultiToResult To<TConcrete>() where TConcrete : class;
        IDiMultiToResult To(Type concreteType);
    }

    public interface IDiMultiBindResult<in TAbstract>
    {
        IDiMultiToResult To<TConcrete>(TConcrete obj) where TConcrete : class, TAbstract;
        IDiMultiToResult To<TConcrete>(Func<IDiContainer, TConcrete> create) where TConcrete : class, TAbstract;
        IDiMultiToResult To<TConcrete>() where TConcrete : class, TAbstract;
        IDiMultiToResult To(Type concreteType);
    }
}