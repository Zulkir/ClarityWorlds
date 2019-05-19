using System;
using System.Collections.Generic;

namespace Clarity.Common.Infra.ActiveModel
{
    public interface IAmBinding
    {
        IAmObject OwnerObject { get; }
        AmBindingFlags Flags { get; }

        string PropertyName { get; }
        string BindingName { get; }
        Type ChildType { get; }
        Type AbstractValueType { get; }

        object GetAbstractValue();
        void SetAbstractValue(object value);
        object CloneAbstractValue(Dictionary<IAmObject, IAmObject> clones);

        // todo: change to DetachChild(object token)
        void DetachChild(IAmObject child);
    }

    public interface IAmBinding<out TOwnerObj> : IAmBinding
    {
        new TOwnerObj OwnerObject { get; }
    }
}