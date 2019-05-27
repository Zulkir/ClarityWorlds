using JetBrains.Annotations;

namespace Clarity.App.Worlds.Assets
{
    public struct AssetLoadResult
    {
        [CanBeNull] public IAsset Asset;
        [CanBeNull] public string Message;

        public bool Successful => Asset != null;

        public static AssetLoadResult Success(IAsset asset) => new AssetLoadResult { Asset = asset };
        public static AssetLoadResult Failure(string message) => new AssetLoadResult { Asset = null, Message = message };
    }
}