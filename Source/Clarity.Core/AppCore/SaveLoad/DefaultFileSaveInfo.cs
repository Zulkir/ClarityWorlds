using System.Collections.Generic;
using System.IO;
using Clarity.Common.Infra.Files;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class DefaultFileSaveInfo : IFileSaveInfo
    {
        public IFileSystem FileSystem { get; }
        public string FilePath { get; }
        public IWorld World { get; }
        private readonly IAssetService assetService;

        public DefaultFileSaveInfo(string filePath, IWorld world, IAssetService assetService)
        {
            FileSystem = new ActualFileSystem();
            FilePath = filePath;
            World = world;
            this.assetService = assetService;
        }

        public IEnumerable<string> GetAssetLocalFiles(IAsset asset) => 
            assetService.GetAssetLocalFiles(asset);

        public Stream ReadAssetFile(IAsset asset, string relativePath) =>
            assetService.ReadAssetFile(asset, relativePath);
    }
}