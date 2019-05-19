using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Clarity.Common.Infra.ActiveModel
{
    public interface IAmObject : ICloneable
    {
        Type AmInterface { get; }
        IAmBinding AmParentBinding { get; }
        IReadOnlyList<IAmBinding> AmReferingBindings { get; }
        object AmParentBindingToken { get; }
        [NotNull] IReadOnlyList<IAmBinding> Bindings { get; }
        void AmCloneNonBoundStateFrom(IAmObject other);
        void Deparent();

        // todo: to an Internal prop
        void InternalOnAdoptedBy(IAmBinding newParentBinding, object token);
        void InternalOnDisowned();
        void InternalOnReferencedBy(IAmBinding newRefererBinding, object token);
        void InternalOnDereferencedBy(IAmBinding refererBinding);
        void InternalOnChildEvent(IAmEventMessage message);
        void InternalOnReferenceEvent(IAmEventMessage message);
        void InternalUpdateToken(object token);
        object InternalClone(Dictionary<IAmObject, IAmObject> clones);

        void InternalInitInstantiator(IAmObjectInstantiator instantiator);
        void InternalRegisterBinding(IAmBinding binding);
    }

    public interface IAmObject<out TParent> : IAmObject
    {
        [CanBeNull] TParent AmParent { get; }
    }
}