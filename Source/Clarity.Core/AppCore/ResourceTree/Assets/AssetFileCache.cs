using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.Common.Infra.Files;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    public class AssetFileCache : IAssetFileCache
    {
        private readonly IFileSystem cacheFileSystem;

        public AssetFileCache()
        {
            cacheFileSystem = new MemoryFileSystem();
        }

        private static string GetFullPath(string assetName, string relativePath) =>
            Path.Combine($"/{assetName}/", relativePath);

        public IEnumerable<string> GetLocalFiles(string assetName) =>
            cacheFileSystem.EnumerateFilesRecursively(assetName)
                .Select(x => new FilePath(x).GetRelativePathFrom(assetName).ToString());

        public Stream CreateCacheFile(string assetName, string relativePath) => 
            cacheFileSystem.OpenWriteNew(GetFullPath(assetName, relativePath));

        public void CopyCacheFileFrom(string assetName, string relativePath, IFileSystem fileSystem, string path)
        {
            using (var writeStream = CreateCacheFile(assetName, relativePath))
            using (var readStream = fileSystem.OpenRead(path))
                readStream.CopyTo(writeStream);
        }

        public Stream ReadCacheFile(string assetName, string relativePath) => 
            cacheFileSystem.OpenRead(GetFullPath(assetName, relativePath));

        public void DeleteAssetFiles(string assetName) => 
            cacheFileSystem.DeleteFolder(assetName);

        public void Clear() => 
            cacheFileSystem.DeleteFolder("/");
    }
}