using System.IO;
using Clarity.Engine.Media.Images;

namespace Clarity.App.Transport.Prototype.Dummy
{
    public class DummyImageLoader : IImageLoader
    {
        public IImage Load(Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}