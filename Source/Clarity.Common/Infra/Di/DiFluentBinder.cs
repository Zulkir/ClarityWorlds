using System;
using System.Reflection;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;

namespace Clarity.Common.Infra.Di
{
    public class DiFluentBinderCommon<TAbstract> :
        IDiFirstBindResult, IDiToResult, 
        IDiFirstBindResult<TAbstract>, IDiToResult<TAbstract>,
        IDiMultiBindResult, IDiMultiToResult,
        IDiMultiBindResult<TAbstract>
    {
        private readonly IDiContainer di;
        private readonly IDiRootBinding rootBinding;

        public DiFluentBinderCommon(IDiContainer di, IDiRootBinding rootBinding)
        {
            this.di = di;
            this.rootBinding = rootBinding;
            if (typeof(TAbstract) != typeof(object) && typeof(TAbstract) != rootBinding.AbstractType)
                throw new ArgumentException("TAbstract generic type must either be 'object' or be the same as the 'abstractType' parameter.");
            rootBinding.MoveToHead();
        }

        #region To
        IDiToResult<TAbstract> IDiBindResult<TAbstract>.To<TConcrete>(TConcrete obj) { InternalTo(obj); return this; }
        IDiToResult<TAbstract> IDiBindResult<TAbstract>.To<TConcrete>(Func<IDiContainer, TConcrete> create) { InternalTo(create); return this; }
        IDiToResult<TAbstract> IDiBindResult<TAbstract>.To<TConcrete>(Func<IDiContainer, Type[], TConcrete> create) { InternalTo(create); return this; }
        IDiToResult IDiBindResult.To(object obj, MethodInfo method) { InternalTo(obj, method); return this; }
        IDiToResult<TAbstract> IDiBindResult<TAbstract>.To<TConcrete>() { InternalTo(typeof(TConcrete)); return this; }
        IDiToResult IDiBindResult<TAbstract>.To(ConstructorInfo constructor) { InternalTo(constructor); return this; }
        IDiToResult IDiBindResult<TAbstract>.To(object obj, MethodInfo method) { InternalTo(obj, method); return this; }
        IDiToResult IDiBindResult<TAbstract>.To(Type concreteType) { InternalTo(concreteType); return this; }
        IDiToResult IDiBindResult.To<TConcrete>(TConcrete obj) { InternalTo(obj); return this; }
        IDiToResult IDiBindResult.To<TConcrete>(Func<IDiContainer, TConcrete> create) { InternalTo(create); return this; }
        IDiToResult IDiBindResult.To(Type concreteType) { InternalTo(concreteType); return this; }
        IDiToResult IDiBindResult.To(ConstructorInfo constructor) { InternalTo(constructor); return this; }
        IDiToResult IDiBindResult.To<TConcrete>() { InternalTo(typeof(TConcrete)); return this; }
        IDiMultiToResult IDiMultiBindResult<TAbstract>.To<TConcrete>(Func<IDiContainer, TConcrete> create) { InternalTo(create); return this; }
        IDiMultiToResult IDiMultiBindResult<TAbstract>.To<TConcrete>() { InternalTo(typeof(TConcrete)); return this; }
        IDiMultiToResult IDiMultiBindResult<TAbstract>.To(Type concreteType) { InternalTo(concreteType); return this; }
        IDiMultiToResult IDiMultiBindResult<TAbstract>.To<TConcrete>(TConcrete obj) { InternalTo(obj); return this; }
        IDiMultiToResult IDiMultiBindResult.To<TConcrete>(Func<IDiContainer, TConcrete> create) { InternalTo(create); return this; }
        IDiMultiToResult IDiMultiBindResult.To<TConcrete>() { InternalTo(typeof(TConcrete)); return this; }
        IDiMultiToResult IDiMultiBindResult.To(Type concreteType) { InternalTo(concreteType); return this; }
        IDiMultiToResult IDiMultiBindResult.To<TConcrete>(TConcrete obj) { InternalTo(obj); return this; }

        private void InternalTo<TConcrete>(TConcrete obj)
        {
            ValidateConcreteType(typeof(TConcrete));
            rootBinding.AddNewCase(new DiObjectBinding(obj));
        }

        private void InternalTo<TConcrete>(Func<IDiContainer, TConcrete> create) where TConcrete : class
        {
            ValidateConcreteType(typeof(TConcrete));
            rootBinding.AddNewCase(new DiFuncBinding(create, typeof(TConcrete)));
        }

        private void InternalTo(Type concreteType)
        {
            ValidateConcreteType(concreteType);

            if (rootBinding.AbstractType == concreteType)
                rootBinding.AddNewCase(new DiConstructorBinding(concreteType));
            else
            {
                di.Bind(concreteType).AsLastChoice.To(concreteType);
                rootBinding.AddNewCase(new DiRedirectBinding(concreteType));
            }
        }

        private void InternalTo(object obj, MethodInfo method)
        {
            var concreteType = method.ReturnType.ContainsGenericParameters
                ? method.ReturnType.GetGenericTypeDefinition()
                : method.ReturnType;

            ValidateConcreteType(concreteType);

            if (rootBinding.AbstractType == concreteType)
                rootBinding.AddNewCase(new DiMethodBinding(obj, method));
            else
            {
                di.Bind(concreteType).AsLastChoice.To(concreteType);
                rootBinding.AddNewCase(new DiRedirectBinding(concreteType));
            }
        }

        private void InternalTo(ConstructorInfo constructor)
        {
            ValidateConcreteType(constructor.DeclaringType);

            if (rootBinding.AbstractType == constructor.DeclaringType)
                rootBinding.AddNewCase(new DiConstructorBinding(constructor));
            else
            {
                di.Bind(constructor.DeclaringType).AsLastChoice.To(constructor.DeclaringType);
                rootBinding.AddNewCase(new DiRedirectBinding(constructor.DeclaringType));
            }
        }
        #endregion

        #region If
        IDiBindResult IDiToResult.If(Func<IDiContainer, bool> condition) { InternalIf(condition); return this; }
        IDiBindResult<TAbstract> IDiToResult<TAbstract>.If(Func<IDiContainer, bool> condition) { InternalIf(condition); return this; }
        void IDiMultiToResult.If(Func<IDiContainer, bool> condition) { InternalIf(condition); }

        private void InternalIf(Func<IDiContainer, bool> condition)
        {
            rootBinding.SetConditionForCurrentCase(condition);
            rootBinding.IncCurrent();
        }
        #endregion

        #region AsLastChoice
        IDiBindResult IDiFirstBindResult.AsLastChoice { get { InternalAsLastChoice(); return this; } }
        IDiBindResult<TAbstract> IDiFirstBindResult<TAbstract>.AsLastChoice { get { InternalAsLastChoice(); return this; } }

        private void InternalAsLastChoice()
        {
            rootBinding.MoveToTail();
        }
        #endregion

        #region Otherwise
        void IDiToResult<TAbstract>.Otherwise() { }
        void IDiToResult.Otherwise() { }
        #endregion

        private void ValidateConcreteType(Type concreteType)
        {
            if (!rootBinding.AbstractType.IsGenericlyAssignableFrom(concreteType))
                    throw new ArgumentException(string.Format(
                        "Trying to bind concrete type '{0}' to abstract type '{1}', while the former does not implement the later.",
                        concreteType.FullName, rootBinding.AbstractType.FullName));
        }
    }
}