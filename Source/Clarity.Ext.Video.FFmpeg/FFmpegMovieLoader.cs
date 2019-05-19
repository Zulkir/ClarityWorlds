using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.Core.AppCore.ResourceTree.Assets;

namespace Clarity.Ext.Video.FFmpeg
{
    public class FFmpegMovieLoader : IAssetLoader
    {
        private static readonly string[] SuppertedExtensions = { ".mkv", ".mp4", ".avi", ".mov", ".flv" };

        public string Name => "ffmpeg.Movies";
        public string AssetTypeString => $"Movie files ({Name})";
        public IReadOnlyList<string> FileExtensions => SuppertedExtensions;
        public AssetLoaderFlags Flags => AssetLoaderFlags.None;

        private readonly FFmpegInitializer initializer;
        private readonly Lazy<IAssetService> assetServiceLazy;

        public FFmpegMovieLoader(FFmpegInitializer initializer, Lazy<IAssetService> assetServiceLazy)
        {
            this.initializer = initializer;
            this.assetServiceLazy = assetServiceLazy;
        }

        public bool LikesName(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return SuppertedExtensions.Contains(extension);
        }

        public AssetLoadResultByLoader Load(AssetLoadInfo loadInfo)
        {
            try
            {
                initializer.EnsureInitialized();
                var asset = CreateAsset(loadInfo);
                return AssetLoadResultByLoader.Success(asset);
            }
            catch (Exception ex)
            {
                return AssetLoadResultByLoader.Failure(ex.GetType().Name, ex);
            }
        }

        private IAsset CreateAsset(AssetLoadInfo loadInfo)
        {
            var fileBytes = loadInfo.FileSystem.ReadAllBytes(loadInfo.LoadPath);

            int width, height;
            double duration;

            using (var stream = new MemoryStream(fileBytes))
            using (var reader = new FFmpegMovieReader(stream))
            {
                width = reader.Width;
                height = reader.Height;
                duration = reader.Duration;
            }

            var assetName = loadInfo.AssetName;
            var fileName = Path.GetFileName(loadInfo.LoadPath);
            var movie = new FFmpegMovie(width, height, duration, () => OpenAssetFile(assetName, fileName));
            var hash = AssetHashMd5.FromSingleFile(fileBytes);
            return new Asset(assetName, movie, loadInfo.StorageType, hash, loadInfo.ReferencePath, fileName);
        }

        private Stream OpenAssetFile(string assetName, string movieFileName)
        {
            var assetService = assetServiceLazy.Value;
            var asset = assetService.Assets[assetName];
            return assetService.ReadAssetFile(asset, movieFileName);
        }
    }
}