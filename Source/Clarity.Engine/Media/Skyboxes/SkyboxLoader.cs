using System.IO;
using System.Linq;
using Clarity.Common.CodingUtilities;
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

        public bool TryLoad(IReadOnlyFileSystem fileSystem, string path, out ISkybox skybox, out string[] imageFileRelativePaths, out ErrorInfo error)
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
            var images = new IImage[6];
            for (var i = 0; i < 6; i++)
            {
                var relPath = imageFileRelativePaths[i];
                using (var stream = fileSystem.OpenRead(Path.Combine(folderPath, relPath)))
                    if (!imageLoader.TryLoad(stream, out images[i], out error))
                    {
                        skybox = null;
                        return false;
                    }
            }

            var width = images[0].Size.Width;
            if (images.Any(x => x.Size != new IntSize2(width, width)))
            {
                error = new ErrorInfo("Skybox images are not of equal size");
                skybox = null;
                return false;
            }
            skybox = new Skybox(ResourceVolatility.Immutable, width, images);
            return true;
        }
    }
}