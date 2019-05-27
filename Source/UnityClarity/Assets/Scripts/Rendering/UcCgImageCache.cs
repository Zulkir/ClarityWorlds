using Assets.Scripts.Assets;
using Clarity.Common.Numericals;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.Caching;
using UnityEngine;

namespace Assets.Scripts.Rendering
{
    public class UcCgImageCache : ICache
    {
        private readonly IImage image;
        private readonly bool isNormalMap;
        private Texture2D unityTexture;
        private bool ownsTexture;
        private bool dirty;

        public bool IsDisposed { get; private set; }

        public UcCgImageCache(IImage image, bool isNormalMap = false)
        {
            this.image = image;
            this.isNormalMap = isNormalMap;
            dirty = true;
        }

        private void DestroyTexture()
        {
            if (ownsTexture && unityTexture == null)
                return;
            // todo: make work
            //Object.Destroy(unityTexture);
            unityTexture = null;
            ownsTexture = false;
        }

        public void Dispose()
        {
            DestroyTexture();
            IsDisposed = true;
        }

        public void OnMasterEvent(object eventArgs)
        {
            dirty = true;
        }

        public unsafe Texture GetUnityTexture()
        {
            if (!dirty)
                return unityTexture;

            DestroyTexture();

            if (image is UcImage)
            {
                var ucImage = (UcImage)image;
                unityTexture = ucImage.UnityTexture;
                ownsTexture = false;
                dirty = false;
                return unityTexture;
            }

            var rowSpan = GraphicsHelper.AlignedRowSpan(image.Size.Width);
            var pixelArray = new Color32[image.Size.Area];
            var rawData = image.GetRawData();
            fixed (byte* pRawData = rawData)
            for (int y = 0; y < image.Size.Height; y++)
            for (int x = 0; x < image.Size.Width; x++)
            {
                var pixel = *(Color32*)(pRawData + (rowSpan * y + sizeof(Color32) * x));
                pixelArray[image.Size.Width * y + x] = pixel;
            }

            unityTexture = new Texture2D(image.Size.Width, image.Size.Height, TextureFormat.RGBA32, true, isNormalMap)
            {
                filterMode = FilterMode.Trilinear,
                anisoLevel = 16
            };
            unityTexture.SetPixels32(pixelArray);
            unityTexture.Apply(true, true);
            ownsTexture = true;

            dirty = false;
            return unityTexture;
        }
    }
}