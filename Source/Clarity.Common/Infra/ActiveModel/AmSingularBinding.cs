using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;

namespace Clarity.Common.Infra.ActiveModel
{
    public class AmSingularBinding<TOwnerObj, TChild> : IAmSingularBinding<TOwnerObj, TChild> 
        where TOwnerObj : class, IAmObject
    {
        private TChild InternalValue { get; set; }
        public TOwnerObj OwnerObject { get; }
        public AmBindingFlags Flags { get; }
        public string PropertyName { get; }

        IAmObject IAmBinding.OwnerObject => OwnerObject;
        public string BindingName => $"{typeof(TOwnerObj).Name}.{PropertyName}";
        public Type ChildType => typeof(TChild);
        public Type AbstractValueType => typeof(TChild);

        private static readonly bool ChildIsAmObject = typeof(IAmObject).IsAssignableFrom(typeof(TChild));

        public AmSingularBinding(TOwnerObj ownerObject, string propertyName, AmBindingFlags flags)
        {
            OwnerObject = ownerObject;
            PropertyName = propertyName;
            Flags = flags;
        }

        public TChild Value { get => GetValue(); set => SetValue(value); }
        public TChild GetValue() => InternalValue;
        public object GetAbstractValue() => GetValue();

        public void SetValue(TChild val)
        {
            var oldValue = InternalValue;
            InternalValue = val;
            if (!Flags.HasFlag(AmBindingFlags.Reference))
            {
                (oldValue as IAmObject)?.InternalOnDisowned();
                (val as IAmObject)?.InternalOnAdoptedBy(this, null);
            }
            else
            {
                (oldValue as IAmObject)?.InternalOnDereferencedBy(this);
                (val as IAmObject)?.InternalOnReferencedBy(this, null);
            }
            var message = new AmValueChangedEventMessage<TOwnerObj, TChild>(this, oldValue, val);
            OwnerObject.InternalOnChildEvent(message);
        }
        
        public void SetAbstractValue(object value)
        {
            if (value == null)
            {
                if (typeof(TChild).IsValueType && !typeof(TChild).IsNullable())
                    throw new ArgumentException("Trying to assign null to a value-typed binding.");
                SetValue(default(TChild));
            }
            else
            {
                if (!(value is TChild))
                    throw new ArgumentException($"Value was expected to be of type '{typeof(TChild)}', but was of type '{value.GetType().Name}'.");
                SetValue((TChild)value);
            }
        }

        public object CloneAbstractValue(Dictionary<IAmObject, IAmObject> clones)
        {
            if (Flags.HasFlag(AmBindingFlags.Derived))
                return default(TChild);
            if (typeof(TChild).IsValueType)
                return InternalValue;
            if (InternalValue == null)
                return null;
            if (InternalValue is IAmObject am)
                return clones.TryGetRef((IAmObject)InternalValue) ?? am.InternalClone(clones);
            if (InternalValue is ICloneable clonable)
                return clonable.Clone();
            return InternalValue;
        }

        public void DetachChild(IAmObject child)
        {
            // todo: debug only?
            if (!ChildIsAmObject)
                throw new InvalidOperationException($"Calling {nameof(DetachChild)}() on a binding '{BindingName}' of child type '{typeof(TChild).Name}' that is not even IAmObject.");
            if (child.AmParentBinding != this)
                throw new InvalidOperationException($"Calling {nameof(DetachChild)}() on a binding '{BindingName}' for an object, which does not have this binding as a parent.");
            var token = child.AmParentBindingToken;
            if (token != null)
                throw new InvalidOperationException($"Incorrect token '{token}' returned to a binding '{BindingName}'. For singular bindings the only valid token is null.");
            if (!ReferenceEquals(child, InternalValue))
                throw new InvalidOperationException($"{nameof(DetachChild)}() was called for a non-attached child.");

            SetValue(default(TChild));
        }
    }
}