using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Media.Skyboxes;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    public class SkyboxAssetLoader : IAssetLoader
    {
        private static readonly string[] SupportedExtensions = {".skybox"};
        public string Name => "CORE.Skybox";
        public string AssetTypeString => "Skyboxes (Core)";
        public IReadOnlyList<string> FileExtensions => SupportedExtensions;
        public AssetLoaderFlags Flags => AssetLoaderFlags.MultiFile | AssetLoaderFlags.ManualCaching;

        private readonly ISkyboxLoader skyboxLoader;
        private readonly IAssetFileCache assetFileCache;

        public SkyboxAssetLoader(IAssetFileCache assetFileCache, ISkyboxLoader skyboxLoader)
        {
            this.assetFileCache = assetFileCache;
            this.skyboxLoader = skyboxLoader;
        }

        public bool LikesName(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return SupportedExtensions.Contains(extension);
        }

        public AssetLoadResultByLoader Load(AssetLoadInfo loadInfo)
        {
            try
            {
                var skybox = skyboxLoader.Load(loadInfo.FileSystem, loadInfo.LoadPath, out var imageFileRelativePathsStr);
                var pathsForHashing = new List<string>();
                var imageRelativePaths = imageFileRelativePathsStr.Select(x => new FilePath(x)).ToArray();
                var paddingCount = imageRelativePaths.Max(x => x.UpCount);
                var loadFolderPath = new FilePath(Path.GetDirectoryName(loadInfo.LoadPath));
                var cacheFolderPath = new FilePath(Enumerable.Range(0, paddingCount).Select(x => "Padding").ToArray());
                var mainFileName = Path.GetFileName(loadInfo.LoadPath);
                var mainFileCachePath = cacheFolderPath.CombineWith(mainFileName).ToString();
                pathsForHashing.Add(mainFileCachePath);

                assetFileCache.CopyCacheFileFrom(loadInfo.AssetName, mainFileCachePath, loadInfo.FileSystem, loadInfo.LoadPath);
                foreach (var imageFileRelativePath in imageRelativePaths)
                {
                    var imageCachePath = cacheFolderPath.CombineWith(imageFileRelativePath).ToString();
                    var imageLoadPath = loadFolderPath.CombineWith(imageFileRelativePath).ToString();
                    assetFileCache.CopyCacheFileFrom(loadInfo.AssetName, imageCachePath, loadInfo.FileSystem, imageLoadPath);
                    pathsForHashing.Add(imageCachePath);
                }

                var hash = AssetHashMd5.FromMultipleFiles(x => assetFileCache.ReadCacheFile(loadInfo.AssetName, x), pathsForHashing);
                var asset = new Asset(loadInfo.AssetName, skybox, loadInfo.StorageType, hash, loadInfo.ReferencePath, mainFileCachePath);
                return AssetLoadResultByLoader.Success(asset);
            }
            catch (Exception ex)
            {
                return AssetLoadResultByLoader.Failure("EXCEPTION", ex);
            }
        }
    }
}