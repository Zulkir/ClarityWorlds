using System.Collections.Generic;
using System.IO;

namespace Clarity.App.Worlds.Assets
{
    public interface IAssetService
    {
        IReadOnlyDictionary<string, IAsset> Assets { get; }
        string DisambiguateName(string assetName);
        IEnumerable<string> GetAssetLocalFiles(IAsset asset);
        Stream ReadAssetFile(IAsset asset, string relativePath);
        AssetLoadResult Load(AssetLoadInfo loadInfo);
        void Delete(IAsset asset);
        void DeleteAll();
        // todo: event Updated (if needed)
    }
}