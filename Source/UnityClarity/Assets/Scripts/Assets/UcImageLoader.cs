using System;
using System.IO;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;
using UnityEngine;

namespace Assets.Scripts.Assets
{
    public class UcImageLoader : IImageLoader
    {
        public IImage Load(Stream stream)
        {
            var texture = new Texture2D(1, 1);
            if (!texture.LoadImage(stream.ReadToEnd(), true))
                throw new Exception("Failed to load image data");
            return new UcImage(texture, ResourceVolatility.Immutable);
        }
    }
}