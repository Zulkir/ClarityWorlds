using System.Collections.Generic;
using System.IO;
using Clarity.Common.Infra.Files;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    public interface IAssetFileCache
    {
        IEnumerable<string> GetLocalFiles(string assetName);
        Stream CreateCacheFile(string assetName, string relativePath);
        void CopyCacheFileFrom(string assetName, string relativePath, IFileSystem fileSystem, string path);
        Stream ReadCacheFile(string assetName, string relativePath);
        void DeleteAssetFiles(string assetName);
        void Clear();
    }
}