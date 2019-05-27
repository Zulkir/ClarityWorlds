using System.Collections.Generic;
using System.IO;
using Clarity.App.Worlds.Assets;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad
{
    public class DefaultFileSaveInfo : IFileSaveInfo
    {
        public IFileSystem FileSystem { get; }
        public string FilePath { get; }
        public IWorld World { get; }
        public IWorld ReadOnlyWorld { get; }
        private readonly IAssetService assetService;

        public bool IncludeEditingWorld => World != null;
        public bool IncludeReadOnlyWorld => ReadOnlyWorld != null;

        public DefaultFileSaveInfo(string filePath, IWorld world, IWorld readOnlyWorld, IAssetService assetService)
        {
            FileSystem = new ActualFileSystem();
            FilePath = filePath;
            World = world;
            ReadOnlyWorld = readOnlyWorld;
            this.assetService = assetService;
        }

        public IEnumerable<string> GetAssetLocalFiles(IAsset asset) => 
            assetService.GetAssetLocalFiles(asset);

        public Stream ReadAssetFile(IAsset asset, string relativePath) =>
            assetService.ReadAssetFile(asset, relativePath);
    }
}