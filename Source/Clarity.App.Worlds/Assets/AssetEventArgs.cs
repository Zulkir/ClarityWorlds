namespace Clarity.App.Worlds.Assets
{
    public class AssetEventArgs
    {
        public AssetEventType Type { get; }
        public IAsset Asset { get; }

        public AssetEventArgs(AssetEventType type, IAsset asset)
        {
            Type = type;
            Asset = asset;
        }
    }
}