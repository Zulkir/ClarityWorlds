using System.Drawing;
using System.IO;
using Clarity.Engine.Media.Images;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    // todo: move to Eto project
    public class SysDrawImageLoader : IImageLoader
    {
        public IImage Load(Stream stream)
        {
            return new SysDrawImage(Image.FromStream(stream));
        }
    }
}