using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;

namespace Clarity.Common.Infra.ActiveModel
{
    public class AmListBinding<TOwnerObj, TChild> : IAmListBinding<TOwnerObj, TChild> 
        where TOwnerObj : class, IAmObject
    {
        public RelaxedObservableList<TChild> Items { get; }
        public TOwnerObj OwnerObject { get; }
        public AmBindingFlags Flags { get; }
        public string PropertyName { get; }
        
        IAmObject IAmBinding.OwnerObject => OwnerObject;
        public string BindingName => $"{typeof(TChild).Name}.{PropertyName}";
        public Type ChildType => typeof(TChild);
        public Type AbstractValueType => typeof(TChild).MakeArrayType();

        private static readonly bool ChildIsAmObject = typeof(IAmObject).IsAssignableFrom(typeof(TChild));

        public AmListBinding(TOwnerObj ownerObject, string propertyName, AmBindingFlags flags)
        {
            OwnerObject = ownerObject;
            PropertyName = propertyName;
            Flags = flags;
            Items = new RelaxedObservableList<TChild>();
            Items.ItemAdded += OnItemAdded;
            Items.ItemRemoved += OnItemRemoved;
        }

        private void OnItemAdded(TChild item, int index)
        {
            if (!Flags.HasFlag(AmBindingFlags.Reference))
                (item as IAmObject)?.InternalOnAdoptedBy(this, index);
            else
                (item as IAmObject)?.InternalOnReferencedBy(this, index);
            var message = new AmItemAddedEventMessage<TOwnerObj, TChild>(this, item);
            OwnerObject.InternalOnChildEvent(message);
        }

        private void OnItemRemoved(TChild item, int index)
        {
            if (!Flags.HasFlag(AmBindingFlags.Reference))
                (item as IAmObject)?.InternalOnDisowned();
            else
                (item as IAmObject)?.InternalOnDereferencedBy(this);
            var message = new AmItemRemovedEventMessage<TOwnerObj, TChild>(this, item);
            OwnerObject.InternalOnChildEvent(message);

            // todo: remove when switched to a list with holes
            for (int i = index; i < Items.Count; i++)
                (Items[i] as IAmObject)?.InternalUpdateToken(i);
        }

        public object GetAbstractValue() => Items.ToArray();

        public void Clear()
        {
            Items.Clear();
        }

        public void AddAbstractItem(object item)
        {
            if (item == null)
                Items.Add(default(TChild));
            else if (!(item is TChild cItem))
                throw new ArgumentException($"Value was expected to be of type '{typeof(TChild).Name}', but was of type '{item.GetType().Name}'.");
            else
                Items.Add(cItem);
        }

        public void SetAbstractValue(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (!(value is TChild[]))
                throw new ArgumentException($"Value was expected to be of type '{typeof(TChild[]).Name}', but was of type '{value.GetType().Name}'.");

            Items.Clear();
            foreach (var item in (TChild[])value)
                Items.Add(item);
        }

        public object CloneAbstractValue(Dictionary<IAmObject, IAmObject> clones)
        {
            return CloneAbstractValueTyped(clones);
        }

        public TChild[] CloneAbstractValueTyped(Dictionary<IAmObject, IAmObject> clones)
        {
            if (Flags.HasFlag(AmBindingFlags.Derived))
                return new TChild[0];
            if (typeof(TChild).IsValueType)
                return Items.ToArray();
            if (typeof(IAmObject).IsAssignableFrom(typeof(TChild)))
                return Items.Select(x => (IAmObject)x).Select(x => clones.TryGetRef(x) ?? x.InternalClone(clones)).Select(x => (TChild)x).ToArray();
            if (typeof(ICloneable).IsAssignableFrom(typeof(TChild)))
                return Items.Select(x => ((ICloneable)x).Clone<TChild>()).ToArray();
            var result = new TChild[Items.Count];
            for (int i = 0; i < Items.Count; i++)
            {
                var cloneable = Items[i] as ICloneable;
                result[i] = cloneable != null ? cloneable.Clone<TChild>() : Items[i];
            }
            return result;
        }

        public void DetachChild(IAmObject child)
        {
            // todo: debug only?
            if (!ChildIsAmObject)
                throw new InvalidOperationException($"Calling {nameof(DetachChild)}() on a binding '{BindingName}' of child type '{typeof(TChild).Name}' that is not even IAmObject.");
            if (child.AmParentBinding != this)
                throw new InvalidOperationException($"Calling {nameof(DetachChild)}() on a binding '{BindingName}' for an object, which does not have this binding as a parent.");
            var token = child.AmParentBindingToken;
            if (!(token is int))
                throw new ArgumentException($"Incorrect token '{token}' returned to a binding '{BindingName}'.", nameof(token));
            // todo: use token
            Items.Remove((TChild)child);
            //return;
            //var index = (int)token;
            //// todo: change to TryGet
            //if (index < 0 || index >= Items.Count)
            //    throw new ArgumentException($"Token '{token}' is not in a valid range.");
            //if (!ReferenceEquals(child, Items[index]))
            //    throw new InvalidOperationException($"Calling {nameof(DetachChild)}() for a non-attached child.");
            //
            //Items.RemoveAt(index);
        }
    }
}