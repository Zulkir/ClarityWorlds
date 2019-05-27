using System;

namespace Clarity.Engine.Resources
{
    public class GeneratedResourceSource : IResourceSource
    {
        public IResource Resource { get; }
        public Type SaveLoadResourceType { get; }

        public IResource GetResource() => Resource;

        public GeneratedResourceSource(IResource resource, Type saveLoadResourceType)
        {
            Resource = resource;
            SaveLoadResourceType = saveLoadResourceType;
        }
    }
}