namespace Clarity.Common.Infra.ActiveModel
{
    public interface IAmSingularBinding : IAmBinding
    {
        
    }

    public interface IAmSingularBinding<TChild> : IAmSingularBinding
    {
        TChild Value { get; set; }
        TChild GetValue();
        void SetValue(TChild val);
    }

    public interface IAmSingularBinding<out TOwnerObj, TChild> : IAmSingularBinding<TChild>, IAmBinding<TOwnerObj>
        where TOwnerObj : IAmObject
    {
    }
}