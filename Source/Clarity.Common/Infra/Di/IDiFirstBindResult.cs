namespace Clarity.Common.Infra.Di
{
    public interface IDiFirstBindResult : IDiBindResult
    {
        IDiBindResult AsLastChoice { get; }
    }

    public interface IDiFirstBindResult<in TAbstract> : IDiBindResult<TAbstract>
    {
         IDiBindResult<TAbstract> AsLastChoice { get; }
    }
}