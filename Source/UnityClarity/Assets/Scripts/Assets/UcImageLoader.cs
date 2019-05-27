using System.IO;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;
using UnityEngine;

namespace Assets.Scripts.Assets
{
    public class UcImageLoader : IImageLoader
    {
        public bool TryLoad(Stream stream, out IImage image, out ErrorInfo error)
        {
            var texture = new Texture2D(1, 1);
            var data = stream.ReadToEnd();
            if (!texture.LoadImage(data, true))
            {
                image = null;
                error = new ErrorInfo("Failed to load image data");
            }
            image = new UcImage(texture, ResourceVolatility.Immutable);
            return true;
        }
    }
}