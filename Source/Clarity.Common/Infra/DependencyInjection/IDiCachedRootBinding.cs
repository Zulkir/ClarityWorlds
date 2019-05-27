namespace Clarity.Common.Infra.DependencyInjection
{
    public interface IDiCachedRootBinding
    {
        object GetResult(IDiContainer di);
        IDiRootBinding GetRootBinding(DiRootBindingType requestedRootBindingType);
        object Instantiate(IDiContainer di);
    }
}