using System.Collections.Generic;
using System.IO;
using Clarity.Common.Infra.Files;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.SaveLoad
{
    public interface IFileSaveInfo
    {
        IFileSystem FileSystem { get; }
        string FilePath { get; }
        IWorld World { get; }
        IEnumerable<string> GetAssetLocalFiles(IAsset asset);
        Stream ReadAssetFile(IAsset asset, string relativePath);
    }
}