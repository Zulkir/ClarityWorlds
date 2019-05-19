namespace Clarity.Common.Infra.Di
{
    public interface IDiCachedRootBinding
    {
        object GetResult(IDiContainer di);
        IDiRootBinding GetRootBinding(DiRootBindingType requestedRootBindingType);
        object Instantiate(IDiContainer di);
    }
}