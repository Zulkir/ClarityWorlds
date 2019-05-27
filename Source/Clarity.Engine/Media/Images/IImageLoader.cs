using System.IO;
using Clarity.Common.CodingUtilities;

namespace Clarity.Engine.Media.Images
{
    public interface IImageLoader
    {
        bool TryLoad(Stream stream, out IImage image, out ErrorInfo error);
    }
}