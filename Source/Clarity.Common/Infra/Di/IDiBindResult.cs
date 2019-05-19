using System;
using System.Reflection;

namespace Clarity.Common.Infra.Di
{
    public interface IDiBindResult
    {
        IDiToResult To<TConcrete>(TConcrete obj) where TConcrete : class;
        IDiToResult To<TConcrete>(Func<IDiContainer, TConcrete> create) where TConcrete : class;
        IDiToResult To(Type concreteType);
        IDiToResult To(ConstructorInfo constructor);
        IDiToResult To(object obj, MethodInfo method);
        IDiToResult To<TConcrete>() where TConcrete : class;
    }

    public interface IDiBindResult<in TAbstract>
    {
        IDiToResult<TAbstract> To<TConcrete>(TConcrete obj) where TConcrete : class, TAbstract;
        IDiToResult<TAbstract> To<TConcrete>(Func<IDiContainer, TConcrete> create) where TConcrete : class, TAbstract;
        IDiToResult<TAbstract> To<TConcrete>(Func<IDiContainer, Type[], TConcrete> create) where TConcrete : class, TAbstract;
        IDiToResult To(Type concreteType);
        IDiToResult To(ConstructorInfo constructor);
        IDiToResult To(object obj, MethodInfo method);
        IDiToResult<TAbstract> To<TConcrete>() where TConcrete : class, TAbstract;
    }
}