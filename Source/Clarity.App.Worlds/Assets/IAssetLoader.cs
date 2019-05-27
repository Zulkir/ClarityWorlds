using System.Collections.Generic;

namespace Clarity.App.Worlds.Assets
{
    public interface IAssetLoader
    {
        string Name { get; }
        string AssetTypeString { get; }
        IReadOnlyList<string> FileExtensions { get; }
        AssetLoaderFlags Flags { get; }
        bool LikesName(string fileName);
        AssetLoadResultByLoader Load(AssetLoadInfo loadInfo);
    }
}