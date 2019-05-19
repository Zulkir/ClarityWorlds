using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Engine.Resources
{
    [TrwSerialize]
    public class EmbeddedResourceSource : IResourceSource
    {
        [TrwSerialize]
        public EmbeddedResourceType ResourceType { get; set; }

        [TrwSerialize]
        public string Path { get; set; }

        private readonly IEmbeddedResources embeddedResources;

        public EmbeddedResourceSource(IEmbeddedResources embeddedResources, EmbeddedResourceType resourceType, string path)
        {
            this.embeddedResources = embeddedResources;
            ResourceType = resourceType;
            Path = path;
        }

        public IResource GetResource()
        {
            switch (ResourceType)
            {
                case EmbeddedResourceType.Image:
                    return embeddedResources.Image(Path);
                case EmbeddedResourceType.Skybox:
                    return embeddedResources.Skybox(Path);
                // todo: model loaders
                case EmbeddedResourceType.FlexibleModel:
                case EmbeddedResourceType.ExplicitModel:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}