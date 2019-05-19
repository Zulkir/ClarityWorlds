using System.Collections.Generic;
using JetBrains.Annotations;

namespace Clarity.Common.Infra.ActiveModel
{
    #region AmObjectEventMessage
    public interface IAmEventMessage
    {
        IAmObject Object { get; }
        IAmBinding Binding { get; }
        [CanBeNull] IAmEventMessage ParentMessage { get; }
        Stack<IAmBinding> BindingStack { get; }
        bool StopPropagation { get; set; }
    }

    public interface IAmEventMessage<out TObj> : IAmEventMessage
        where TObj : IAmObject
    {
        new TObj Object { get; }
    }

    public interface IAmEventMessage<out TObj, out TBinding> : IAmEventMessage<TObj>
        where TObj : IAmObject
        where TBinding : IAmBinding<TObj>
    {
        new TBinding Binding { get; }
    }

    public abstract class AmEventMessageBase<TObj, TBinding> : IAmEventMessage<TObj, TBinding>
        where TObj : IAmObject
        where TBinding : IAmBinding<TObj>
    {
        public TBinding Binding { get; }
        public IAmEventMessage ParentMessage { get; }
        public Stack<IAmBinding> BindingStack { get; }
        public bool StopPropagation { get; set; }

        public TObj Object => Binding.OwnerObject;
        IAmObject IAmEventMessage.Object => Object;
        IAmBinding IAmEventMessage.Binding => Binding;

        protected AmEventMessageBase(TBinding binding, IAmEventMessage parentMessage)
        {
            Binding = binding;
            ParentMessage = parentMessage;
            BindingStack = new Stack<IAmBinding>();
            BindingStack.Push(binding);
        }
    }
    #endregion

    #region AmValueChangedEventMessage
    public interface IAmValueChangedEventMessage<out TObj, TChild> : IAmEventMessage<TObj, IAmSingularBinding<TObj, TChild>>
        where TObj : IAmObject
    {
        TChild OldValue { get; }
        TChild NewValue { get; }
    }

    public class AmValueChangedEventMessage<TObj, TChild> : AmEventMessageBase<TObj, IAmSingularBinding<TObj, TChild>>, IAmValueChangedEventMessage<TObj, TChild>
        where TObj : IAmObject
    {
        public TChild OldValue { get; }
        public TChild NewValue { get; }

        public AmValueChangedEventMessage(IAmSingularBinding<TObj, TChild> binding, TChild oldValue, TChild newValue, IAmEventMessage parentMessage = null)
            : base(binding, parentMessage)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
    #endregion

    #region AmItemEventMessage
    public interface IAmItemEventMessage<out TObj, TChild> : IAmEventMessage<TObj, IAmListBinding<TObj, TChild>>
        where TObj : IAmObject
    {
        TChild Item { get; }
        // todo: Key
    }
    #endregion

    #region AmItemAddedEventMessage
    public interface IAmItemAddedEventMessage<out TObj, TChild> : IAmItemEventMessage<TObj, TChild>
        where TObj : IAmObject
    {
    }

    public class AmItemAddedEventMessage<TObj, TChild> : AmEventMessageBase<TObj, IAmListBinding<TObj, TChild>>, IAmItemAddedEventMessage<TObj, TChild>
        where TObj : IAmObject
    {
        public TChild Item { get; }

        public AmItemAddedEventMessage(IAmListBinding<TObj, TChild> binding, TChild item, IAmEventMessage parentMessage = null)
            : base(binding, parentMessage)
        {
            Item = item;
        }
    }
    #endregion

    #region AmItemRemovedEventMessage
    public interface IAmItemRemovedEventMessage<out TObj, TChild> : IAmItemEventMessage<TObj, TChild>
        where TObj : IAmObject
    {
    }

    public class AmItemRemovedEventMessage<TObj, TChild> : AmEventMessageBase<TObj, IAmListBinding<TObj, TChild>>, IAmItemRemovedEventMessage<TObj, TChild>
        where TObj : IAmObject
    {
        public TChild Item { get; }

        public AmItemRemovedEventMessage(IAmListBinding<TObj, TChild> binding, TChild item, IAmEventMessage parentMessage = null)
            : base(binding, parentMessage)
        {
            Item = item;
        }
    }
    #endregion
}