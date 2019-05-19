namespace Clarity.Engine.Resources
{
    public abstract class ResourceFactoryBase : IResourceFactory
    {
        protected IFactoryResourceCache FactoryResourceCache { get; }

        protected ResourceFactoryBase(IFactoryResourceCache factoryResourceCache)
        {
            FactoryResourceCache = factoryResourceCache;
        }

        public abstract string UniqueName { get; }
        public abstract IResource GetNew(IResourceSource resourceSource);
    }
}