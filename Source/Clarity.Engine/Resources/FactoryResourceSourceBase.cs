using JetBrains.Annotations;

namespace Clarity.Engine.Resources
{
    public abstract class FactoryResourceSourceBase<TSelf, TFactory> : IFactoryResourceSource
        where TSelf : class
        where TFactory : class, IResourceFactory
    {
        private readonly IFactoryResourceCache factoryResourceCache;

        public IResourceFactory Factory { get; }
        
        protected FactoryResourceSourceBase(IFactoryResourceCache factoryResourceCache, TFactory factory)
        {
            this.factoryResourceCache = factoryResourceCache;
            Factory = factory;
        }

        public IResource GetResource() => factoryResourceCache.GetOrAdd(this);

        protected abstract bool FieldsAreEqual([NotNull] TSelf other);
        protected abstract int GetFieldsHashCode();

        public override bool Equals(object obj) => obj is TSelf asThis && FieldsAreEqual(asThis);
        public bool Equals(IFactoryResourceSource other) => Equals(other as object);
        public override int GetHashCode() => typeof(TSelf).GetHashCode() ^ GetFieldsHashCode();
    }
}