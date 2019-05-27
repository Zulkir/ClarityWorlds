using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Assets.Scripts.Rendering
{
    public class UcRenderTextureImage : ResourceBase, IImage
    {
        public IntSize2 Size { get; }
        public bool HasTransparency { get; }
        public RenderTexture Texture { get; }

        public UcRenderTextureImage(IntSize2 size, bool hasTransparency = false)
            : base(ResourceVolatility.Volatile)
        {
            Size = size;
            HasTransparency = hasTransparency;
            Texture = new RenderTexture(Size.Width, Size.Height, 24, GraphicsFormat.R8G8B8A8_SRGB);
        }

        public byte[] GetRawData()
        {
            var readableTexture = new Texture2D(Size.Width, Size.Height, GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None);
            RenderTexture.active = Texture;
            readableTexture.ReadPixels(new Rect(0, 0, Size.Width, Size.Height), 0, 0);
            RenderTexture.active = null;
            return readableTexture.GetRawTextureData();
        }
    }
}
