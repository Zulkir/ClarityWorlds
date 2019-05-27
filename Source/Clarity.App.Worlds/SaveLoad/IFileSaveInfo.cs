using System.Collections.Generic;
using System.IO;
using Clarity.App.Worlds.Assets;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad
{
    public interface IFileSaveInfo
    {
        bool IncludeEditingWorld { get; }
        bool IncludeReadOnlyWorld { get; }

        IFileSystem FileSystem { get; }
        string FilePath { get; }
        IWorld World { get; }
        IWorld ReadOnlyWorld { get; }
        IEnumerable<string> GetAssetLocalFiles(IAsset asset);
        Stream ReadAssetFile(IAsset asset, string relativePath);
    }
}