using System;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.SaveLoad;

namespace Clarity.Engine.Media.Images
{
    public class ImageResourceLoader : IResourceLoader
    {
        private readonly IImageLoader imageLoader;

        public ImageResourceLoader(IImageLoader imageLoader)
        {
            this.imageLoader = imageLoader;
        }

        public bool TryLoad(Type type, IFileSystem fileSystem, string path, out IResource resource)
        {
            if (type != typeof(IImage))
            {
                resource = null;
                return false;
            }

            using (var stream = fileSystem.OpenRead(path))
            {
                if (imageLoader.TryLoad(stream, out var image, out _))
                {
                    resource = image;
                    return true;
                }
                
                // todo: write log
                resource = null;
                return false;
            }
        }
    }
}