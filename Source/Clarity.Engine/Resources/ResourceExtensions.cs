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
    }
}