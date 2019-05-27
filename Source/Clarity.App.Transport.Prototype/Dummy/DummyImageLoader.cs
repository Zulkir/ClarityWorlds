using System.IO;
using Clarity.Common.CodingUtilities;
using Clarity.Engine.Media.Images;

namespace Clarity.App.Transport.Prototype.Dummy
{
    public class DummyImageLoader : IImageLoader
    {
        public bool TryLoad(Stream stream, out IImage image, out ErrorInfo error)
        {
            image = null;
            error = new ErrorInfo("Trying to use dummy image loader.");
            return false;
        }
    }
}