using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.Engine.Media.Images;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    public class ImageAssetLoader : IAssetLoader
    {
        private static readonly string[] SupportedExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        public string Name => "Presentations.Default.Image";
        public string AssetTypeString => "Image files (default loader)";
        public IReadOnlyList<string> FileExtensions => SupportedExtensions;
        public AssetLoaderFlags Flags => AssetLoaderFlags.None;

        private readonly IImageLoader imageLoader;

        public ImageAssetLoader(IImageLoader imageLoader)
        {
            this.imageLoader = imageLoader;
        }

        public bool LikesName(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return SupportedExtensions.Contains(extension);
        }

        public AssetLoadResultByLoader Load(AssetLoadInfo loadInfo)
        {
            IImage image;
            var loadPath = loadInfo.LoadPath;
            var fileData = loadInfo.FileSystem.ReadAllBytes(loadPath);
            using (var stream = new MemoryStream(fileData))
                image = imageLoader.Load(stream);
            var hash = AssetHashMd5.FromSingleFile(fileData);
            var asset = new Asset(loadInfo.AssetName, image, loadInfo.StorageType, hash, loadInfo.ReferencePath, Path.GetFileName(loadInfo.ReferencePath));
            return AssetLoadResultByLoader.Success(asset);
        }
    }
}