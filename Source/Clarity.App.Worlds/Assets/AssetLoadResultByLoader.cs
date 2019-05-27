using System;
using Clarity.Common.CodingUtilities;
using JetBrains.Annotations;

namespace Clarity.App.Worlds.Assets
{
    public struct AssetLoadResultByLoader
    {
        [CanBeNull] public IAsset Asset;
        [NotNull] public string AssetName;
        [NotNull] public string ShortMessage;
        [NotNull] public string Message;
        [CanBeNull] public Exception Exception;

        public bool Successful => Asset != null;

        public static AssetLoadResultByLoader Success(IAsset asset) => new AssetLoadResultByLoader
        {
            Asset = asset, 
            ShortMessage = "SUCCESS",
            Message = "SUCCESS"
        };

        public static AssetLoadResultByLoader Failure(string shortMessage, Exception exception) => new AssetLoadResultByLoader
        {
            Asset = null, 
            ShortMessage = shortMessage,
            Message = exception.Message,
            Exception = exception
        };

        public static AssetLoadResultByLoader Failure(string shortMessage, ErrorInfo error) => new AssetLoadResultByLoader
        {
            Asset = null, 
            ShortMessage = shortMessage,
            Message = error.Message,
            Exception = error.Exception
        };
    }
}