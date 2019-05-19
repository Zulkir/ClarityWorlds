using System.Data;
using System.IO;
using System.Linq;
using Clarity.Common.Infra.Files;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Skyboxes
{
    public class SkyboxLoader : ISkyboxLoader
    {
        private readonly IImageLoader imageLoader;
        private readonly ITrwFactory trwFactory;

        public SkyboxLoader(IImageLoader imageLoader, ITrwFactory trwFactory)
        {
            this.imageLoader = imageLoader;
            this.trwFactory = trwFactory;
        }

        public ISkybox Load(IReadOnlyFileSystem fileSystem, string path, out string[] imageFileRelativePaths)
        {
            dynamic mainFile;
            using (var reader = trwFactory.JsonReader(fileSystem.OpenRead(path)))
                mainFile = reader.ReadAsDynamic();
            imageFileRelativePaths = new string[]
            {
                mainFile.Right,
                mainFile.Left,
                mainFile.Top,
                mainFile.Bottom,
                mainFile.Back,
                mainFile.Front
            };
            var folderPath = Path.Combine(Path.GetDirectoryName(path));
            var images = imageFileRelativePaths.Select(x =>
            {
                using (var stream = fileSystem.OpenRead(Path.Combine(folderPath, x)))
                    return imageLoader.Load(stream);
            }).ToArray();
            var width = images[0].Size.Width;
            if (images.Any(x => x.Size != new IntSize2(width, width)))
                throw new DataException("Skybox images are not of equal size");
            return new Skybox(ResourceVolatility.Immutable, width, images);
        }
    }
}