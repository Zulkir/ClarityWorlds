using System;
using JetBrains.Annotations;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    public struct AssetLoadResultByLoader
    {
        [CanBeNull] public IAsset Asset;
        [NotNull] public string AssetName;
        [NotNull] public string ShortMessage;
        [CanBeNull] public Exception Exception;

        public bool Successful => Asset != null;

        public static AssetLoadResultByLoader Success(IAsset asset) => new AssetLoadResultByLoader { Asset = asset, ShortMessage = "SUCCESS" };
        public static AssetLoadResultByLoader Failure(string shortMessage, Exception exception) => new AssetLoadResultByLoader { Asset = null, ShortMessage = shortMessage, Exception = exception};
    }
}