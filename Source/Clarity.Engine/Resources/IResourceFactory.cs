namespace Clarity.Engine.Resources
{
    public interface IResourceFactory
    {
        string UniqueName { get; }
        IResource GetNew(IResourceSource source);
    }
}