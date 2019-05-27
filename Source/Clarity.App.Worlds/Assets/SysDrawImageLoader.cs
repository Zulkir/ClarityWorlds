using System;
using System.Drawing;
using System.IO;
using Clarity.Common.CodingUtilities;
using Clarity.Engine.Media.Images;

namespace Clarity.App.Worlds.Assets
{
    // todo: move to Eto project
    public class SysDrawImageLoader : IImageLoader
    {
        public bool TryLoad(Stream stream, out IImage image, out ErrorInfo error)
        {
            try
            {
                image = new SysDrawImage(Image.FromStream(stream));
                return true;
            }
            catch (Exception ex)
            {
                image = null;
                error = new ErrorInfo(ex);
                return false;
            }
        }
    }
}