using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Clarity.App.Worlds.Logging;
using Clarity.Common.CodingUtilities.Exceptions;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;

namespace Clarity.App.Worlds.Assets
{
    public class AssetService : IAssetService
    {
        private readonly IReadOnlyList<IAssetLoader> assetLoaders;
        private readonly IAssetFileCache assetFileCache;
        private readonly Dictionary<string, IAsset> assets;

        public IReadOnlyDictionary<string, IAsset> Assets => assets;

        public AssetService(IReadOnlyList<IAssetLoader> assetLoaders, IAssetFileCache assetFileCache)
        {
            this.assetLoaders = assetLoaders.Reverse().ToArray();
            this.assetFileCache = assetFileCache;
            assets = new Dictionary<string, IAsset>();
            // todo: UserNotificationService
            if (assetLoaders.Any(x => x.Flags.HasFlag(AssetLoaderFlags.MultiFile) && !x.Flags.HasFlag(AssetLoaderFlags.ManualCaching)))
                throw new TypeContractException("All multi-file loaders must do manual caching.");
        }

        public string DisambiguateName(string assetName)
        {
            var disambiguator = 1;
            var nameToTry = assetName;
            while (assets.ContainsKey(nameToTry))
                nameToTry = $"{assetName}{disambiguator++}";
            return nameToTry;
        }

        public IEnumerable<string> GetAssetLocalFiles(IAsset asset)
        {
            return assetFileCache.GetLocalFiles(asset.Name);
        }

        public Stream ReadAssetFile(IAsset asset, string relativePath)
        {
            switch (asset.StorageType)
            {
                case AssetStorageType.CopyLocal:
                    return assetFileCache.ReadCacheFile(asset.Name, Path.Combine(Path.GetDirectoryName(asset.AssetCachePath), relativePath));
                case AssetStorageType.ReferenceOriginal:
                    return File.OpenRead(Path.Combine(Path.GetDirectoryName(asset.ReferencePath), relativePath));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public AssetLoadResult Load(AssetLoadInfo loadInfo)
        {
            var loadPath = loadInfo.LoadPath;
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Loading " + loadPath);
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Loader responses:");

            if (loadInfo.AssetName != null)
            {
                // todo: check if need handle duplicate names here
                if (assets.ContainsKey(loadInfo.AssetName))
                    throw new ArgumentException($"Asset with the name '{loadInfo.AssetName}' is already registered.");
            }
            else
                // todo: make AssetLoadInfoForLoader
                loadInfo.AssetName = GetUniqueAssetName(Path.GetFileNameWithoutExtension(loadPath));

            var fileName = Path.GetFileName(loadPath);
            foreach (var loader in assetLoaders)
            {
                // todo: decide on this check in config
                if (!loader.LikesName(fileName))
                {
                    messageBuilder.AppendLine($"{loader.Name}: wrong file extension");
                    continue;
                }
                var loaderResult = loader.Load(loadInfo);
                if (loaderResult.Successful)
                {
                    var asset = loaderResult.Asset;
                    if (!loader.Flags.HasFlag(AssetLoaderFlags.ManualCaching))
                        assetFileCache.CopyCacheFileFrom(loadInfo.AssetName, asset.AssetCachePath, loadInfo.FileSystem, loadPath);
                    assets.Add(loadInfo.AssetName, asset);
                    return AssetLoadResult.Success(asset);
                }
                messageBuilder.AppendLine($"{loader.Name}: {FilterLoaderMessage(loaderResult.ShortMessage)}");
                if (loaderResult.Exception != null)
                    Log.Write(LogMessageType.HandledError, $"Exception loading asset '{loadPath}':\r\n" + loaderResult.Exception.GetCompleteMessage());
                assetFileCache.DeleteAssetFiles(loaderResult.AssetName);
            }

            messageBuilder.AppendLine();
            messageBuilder.AppendLine("No loader succeded.");
            return AssetLoadResult.Failure(messageBuilder.ToString());
        }

        private string GetUniqueAssetName(string nameSuggestion)
        {
            if (!assets.ContainsKey(nameSuggestion))
                return nameSuggestion;
            var disambiguator = 2;
            while (assets.ContainsKey($"{nameSuggestion}_{disambiguator}"))
                disambiguator++;
            return $"{nameSuggestion}_{disambiguator}";
        }

        private static string FilterLoaderMessage(string loaderMessage)
        {
            var cutOff = loaderMessage.Length;
            cutOff = (int)Math.Min(cutOff, (uint)loaderMessage.IndexOf('\n'));
            cutOff = (int)Math.Min(cutOff, (uint)loaderMessage.IndexOf('\r'));
            cutOff = (int)Math.Min(cutOff, 16);
            var substring = loaderMessage.Substring(cutOff);
            return cutOff == loaderMessage.Length ? substring : substring + "...";
        }

        public void Delete(IAsset asset)
        {
            assetFileCache.DeleteAssetFiles(asset.Name);
            assets.Remove(asset.Name);
        }

        public void DeleteAll()
        {
            assetFileCache.Clear();
            assets.Clear();
        }
    }
}