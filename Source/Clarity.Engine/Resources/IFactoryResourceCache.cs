namespace Clarity.Engine.Resources
{
    public interface IFactoryResourceCache
    {
        IResource GetOrAdd(IFactoryResourceSource source);
    }
}