using Clarity.Common.CodingUtilities.Collections;

namespace Clarity.Common.Infra.ActiveModel
{
    public interface IAmListBinding : IAmBinding
    {
        void Clear();
        void AddAbstractItem(object item);
    }

    public interface IAmListBinding<TChild> : IAmListBinding
    {
        RelaxedObservableList<TChild> Items { get; }
    }

    public interface IAmListBinding<out TOwnerObj, TChild> : IAmListBinding<TChild>, IAmBinding<TOwnerObj>
        where TOwnerObj : IAmObject
    {
    }
}