using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Clarity.Common.CodingUtilities.Comparers;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Expressions;
using Clarity.Common.Infra.ActiveModel.JitsuGen;
using JitsuGen.Core;

namespace Clarity.Common.Infra.ActiveModel
{
    [JitsuGenIgnore]
    public abstract class AmObjectBase<TAmClass> : AmObjectBase<TAmClass, IAmObject>
        where TAmClass : IAmObject
    {
    }

    [JitsuGen(typeof(AmCodeGenerator))]
    [JitsuGenIgnore]
    public abstract class AmObjectBase<TAmClass, TParentObj> : IAmObject<TParentObj>
        where TParentObj : class, IAmObject
        where TAmClass : IAmObject
    {
        private IAmObjectInstantiator<TAmClass> instantiator;
        private readonly List<IAmBinding> bindings;
        private readonly List<IAmBinding> referingBindings;

        public IAmBinding<TParentObj> AmParentBinding { get; private set; }
        public IReadOnlyList<IAmBinding> AmReferingBindings => referingBindings;
        public object AmParentBindingToken { get; private set; }

        public IReadOnlyList<IAmBinding> Bindings => bindings;
        public TParentObj AmParent => AmParentBinding?.OwnerObject;
        public Type AmInterface => typeof(TAmClass);

        IAmBinding IAmObject.AmParentBinding => AmParentBinding;
        
        protected AmObjectBase()
        {
            bindings = AmInitBindings();
            referingBindings = new List<IAmBinding>();
        }

        // todo: make array instead
        protected abstract List<IAmBinding> AmInitBindings();

        public virtual void AmOnAttached() { }
        public virtual void AmOnChildEvent(IAmEventMessage message) { }
        protected virtual void AmCloneNonBoundStateFrom(TAmClass other) { }

        public void InternalInitInstantiator(IAmObjectInstantiator instantiator)
        {
            this.instantiator = (IAmObjectInstantiator<TAmClass>)instantiator;
        }

        public void Deparent()
        {
            AmParentBinding?.DetachChild(this);
        }

        public bool AmIsDescendantOf(IAmObject amObject)
        {
            return AmParent == amObject || (AmParent?.AmIsDescendantOf(amObject) ?? false);
        }

        public object Clone()
        {
            return InternalClone(new Dictionary<IAmObject, IAmObject>(ReferenceEqualityComparer.Singleton));
        }

        public object InternalClone(Dictionary<IAmObject, IAmObject> clones)
        {
            var clone = instantiator.Instantiate();
            clones.Add(this, clone);
            for (int i = 0; i < Bindings.Count; i++)
            {
                var val = Bindings[i].CloneAbstractValue(clones);
                clone.Bindings[i].SetAbstractValue(val);
            }
            clone.AmCloneNonBoundStateFrom(this);
            return clone;
        }

        public IAmBinding AmGetBinding(string propertyName)
        {
            return Bindings.First(x => x.PropertyName == propertyName);
        }

        public void AmCloneNonBoundStateFrom(IAmObject other) =>
            AmCloneNonBoundStateFrom((TAmClass)other);

        public IAmSingularBinding<T> AmGetBinding<T>(Expression<Func<TAmClass, T>> path)
        {
            var propertyName = path.GetPropertyName();
            return (IAmSingularBinding<T>)AmGetBinding(propertyName);
        }

        public IAmListBinding<T> AmGetBinding<T>(Expression<Func<TAmClass, IList<T>>> path)
        {
            var propertyName = path.GetPropertyName();
            return (IAmListBinding<T>)AmGetBinding(propertyName);
        }

        public void InternalOnAdoptedBy(IAmBinding newParentBinding, object token)
        {
            if (AmParentBinding != null)
                throw new InvalidOperationException($"Trying to implicitly re-parent an AmObject. Deparent the object explicitly by calling AmObject.{nameof(IAmObject.Deparent)}.");
            if (newParentBinding != null && !(newParentBinding is IAmBinding<TParentObj>))
                throw new InvalidOperationException(
                    $"Trying to attach an object with a fixed parent type of '{typeof(TParentObj).Name}' to an object of type '{newParentBinding.GetType().Name}'");
            AmParentBinding = (IAmBinding<TParentObj>)newParentBinding;
            AmParentBindingToken = token;
            AmOnAttached();
        }

        public void InternalOnDisowned()
        {
            AmParentBinding = null;
            AmParentBindingToken = null;
            AmOnAttached();
        }

        public void InternalOnReferencedBy(IAmBinding newRefererBinding, object token)
        {
            referingBindings.Add(newRefererBinding);
        }

        public void InternalOnDereferencedBy(IAmBinding refererBinding)
        {
            referingBindings.Remove(refererBinding);
        }

        public void InternalOnChildEvent(IAmEventMessage message)
        {
            AmOnChildEvent(message);
            if (!message.StopPropagation)
                foreach (var referer in referingBindings)
                    referer.OwnerObject.InternalOnReferenceEvent(message);
            if (!message.StopPropagation)
                AmParent?.InternalOnChildEvent(message);
        }

        public void InternalOnReferenceEvent(IAmEventMessage message)
        {
            AmOnChildEvent(message);
        }

        public void InternalUpdateToken(object token) =>
            AmParentBindingToken = token;

        public void InternalRegisterBinding(IAmBinding binding) =>
            bindings.Add(binding);
    }
}