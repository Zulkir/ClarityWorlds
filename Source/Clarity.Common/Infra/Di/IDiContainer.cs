using System;
using System.Collections.Generic;

namespace Clarity.Common.Infra.Di
{
    public interface IDiContainer
    {
        T Get<T>();
        object Get(Type type);
        IDiFirstBindResult<T> Bind<T>();
        IDiFirstBindResult Bind(Type type);

        IReadOnlyList<T> GetMulti<T>();
        IReadOnlyList<object> GetMulti(Type type);
        IDiMultiBindResult<T> BindMulti<T>();
        IDiMultiBindResult BindMulti(Type type);

        T Instantiate<T>();
        object Instantiate(Type type);

        IDiRootBinding GetRootBinding(Type type, DiRootBindingType rootBindingType);
    }
}