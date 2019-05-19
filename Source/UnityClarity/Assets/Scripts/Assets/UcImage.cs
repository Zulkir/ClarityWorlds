using System;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;
using UnityEngine;

namespace Assets.Scripts.Assets
{
    public class UcImage : ResourceBase, IImage
    {
        public Texture2D UnityTexture { get; }
        public IntSize2 Size { get; }
        public bool HasTransparency { get; }

        public UcImage(Texture2D unityTexture, ResourceVolatility volatility) 
            : base(volatility)
        {
            UnityTexture = unityTexture;
            Size = new IntSize2(UnityTexture.width, UnityTexture.height);
            // todo: HasTransparency
            //HasTransparency = UnityTexture.alphaIsTransparency;
        }

        public byte[] GetRawData()
        {
            throw new NotSupportedException("Read-back of Unity textures is not supported.");
        }
    }
}