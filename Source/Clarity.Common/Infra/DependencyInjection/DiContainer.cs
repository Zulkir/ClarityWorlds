using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clarity.Common.Infra.DependencyInjection
{
    public class DiContainer : IDiContainer
    {
        private static readonly MethodInfo GetFuncMethod = typeof(DiContainer).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.Name == "GetFunc");
        private static readonly MethodInfo GetLazyMethod = typeof(DiContainer).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.Name == "GetLazy");
        private static readonly MethodInfo GetRolMethod = typeof(DiContainer).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.Name == "GetRol");
        private static readonly object[] EmptyObjects = { };

        private readonly ConcurrentDictionary<Type, IDiCachedRootBinding> cachedBindings;

        public DiContainer()
        {
            cachedBindings = new ConcurrentDictionary<Type, IDiCachedRootBinding>();
            Bind<IDiContainer>().To(this);
            Bind(typeof(Func<>)).To(this, GetFuncMethod);
            Bind(typeof(Lazy<>)).To(this, GetLazyMethod);
            Bind(typeof(IReadOnlyList<>)).To(this, GetRolMethod);
        }

        public IDiFirstBindResult<T> Bind<T>() => InternalBind<T>(DiRootBindingType.Single);
        public IDiMultiBindResult<T> BindMulti<T>() => InternalBind<T>(DiRootBindingType.Multi);
        public IDiFirstBindResult Bind(Type type) => InternalBind(type, DiRootBindingType.Single);
        public IDiMultiBindResult BindMulti(Type type) => InternalBind(type, DiRootBindingType.Multi);

        private DiFluentBinderCommon<T> InternalBind<T>(DiRootBindingType rootBindingType) => 
            new DiFluentBinderCommon<T>(this, GetRootBinding(typeof(T), rootBindingType));

        private DiFluentBinderCommon<object> InternalBind(Type type, DiRootBindingType rootBindingType) => 
            new DiFluentBinderCommon<object>(this, GetRootBinding(type, rootBindingType));

        public IDiRootBinding GetRootBinding(Type type, DiRootBindingType rootBindingType) => 
            GetOrAddCachedBinding(type, rootBindingType).GetRootBinding(rootBindingType);

        public T Get<T>() => (T)InternalGet(typeof(T), DiRootBindingType.Single);
        public object Get(Type type) => InternalGet(type, DiRootBindingType.Single);
        public IReadOnlyList<T> GetMulti<T>() => (IReadOnlyList<T>)InternalGet(typeof(T), DiRootBindingType.Multi);
        public IReadOnlyList<object> GetMulti(Type type) => (IReadOnlyList<object>)InternalGet(type, DiRootBindingType.Multi);

        private object InternalGet(Type type, DiRootBindingType rootBindingType) => 
            GetOrAddCachedBinding(type, rootBindingType).GetResult(this);

        private IDiCachedRootBinding GetOrAddCachedBinding(Type type, DiRootBindingType rootBindingType)
        {
            return cachedBindings.GetOrAdd(type, t => new DiCachedRootBinding(new DiRootBinding(t), rootBindingType));
        }
        
        public T Instantiate<T>() => (T)InternalInstantiate(typeof(T), DiRootBindingType.Single);
        public object Instantiate(Type type) => InternalInstantiate(type, DiRootBindingType.Single);

        private object InternalInstantiate(Type type, DiRootBindingType rootBindingType) => 
            GetOrAddCachedBinding(type, rootBindingType).Instantiate(this);

        // ReSharper disable UnusedMember.Local
        private Func<T> GetFunc<T>() { return Get<T>; }
        private Lazy<T> GetLazy<T>() { return new Lazy<T>(GetFunc<T>()); }
        private IReadOnlyList<T> GetRol<T>() { return GetMulti<T>(); }
        // ReSharper restore UnusedMember.Local
    }
}