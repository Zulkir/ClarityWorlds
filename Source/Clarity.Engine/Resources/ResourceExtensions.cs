using System;

namespace Clarity.Engine.Resources
{
    public static class ResourceExtensions
    {
        public static T WithSource<T>(this T resource, IResourceSource source)
            where T : IResource
        {
            resource.Source = source;
            return resource;
        }

        public static T WithSource<T>(this T resource, Func<T, IResourceSource> createSource)
            where T : IResource
        {
            resource.Source = createSource(resource);
            return resource;
        }
    }
}