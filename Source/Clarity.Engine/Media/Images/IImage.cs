using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Images
{
    public interface IImage : IResource, IPixelSource
    {
        IntSize2 Size { get; }
        // todo: image format
        byte[] GetRawData();
    }
}