namespace Clarity.Engine.Resources
{
    // todo: get rid of packs in cases when one resource is main
    public class ResourcePack : ResourceBase
    {
        public IResource MainSubresource { get; set; }

        public ResourcePack(ResourceVolatility volatility) 
            : base(volatility)
        {
        }
    }
}