using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Engine.Resources
{
    [TrwSerialize]
    public class SubresourceResourceSource : IResourceSource
    {
        [TrwSerialize]
        public IResource Parent { get; set; }

        [TrwSerialize]
        public string Key { get; set; }

        public SubresourceResourceSource() { }

        public SubresourceResourceSource(IResource parent, string key)
        {
            Parent = parent;
            Key = key;
        }

        public IResource GetResource()
        {
            return Parent.Subresources[Key];
        }
    }
}