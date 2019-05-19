using System.IO;

namespace Clarity.Engine.Media.Images
{
    public interface IImageLoader
    {
        IImage Load(Stream stream);
    }
}