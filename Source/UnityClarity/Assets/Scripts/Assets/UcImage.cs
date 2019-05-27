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

            HasTransparency = false;
            var data = GetRawData();
            for (var i = 3; i < data.Length; i += 4)
            {
                if (data[i] != 255)
                {
                    HasTransparency = true;
                    break;
                }
            }
        }
        
        public byte[] GetRawData()
        {
            var data = new byte[UnityTexture.width * UnityTexture.height * 4];
            var rawData = UnityTexture.GetRawTextureData();

            switch (UnityTexture.format)
            {
                case TextureFormat.RGB24:
                {
                    for (var i = 0; i < data.Length / 4; i++)
                    {
                        var offset4 = i * 4;
                        var offset3 = i * 3;
                        data[offset4] = rawData[offset3];
                        data[offset4 + 1] = rawData[offset3 + 1];
                        data[offset4 + 2] = rawData[offset3 + 2];
                        data[offset4 + 3] = 255;
                    }
                    break;
                }
                case TextureFormat.RGBA32:
                {
                    Array.Copy(rawData, data, data.Length);
                    break;
                }
                case TextureFormat.ARGB32:
                {
                    for (var i = 0; i < data.Length / 4; i++)
                    {
                        var offset = i * 4;
                        data[offset] = rawData[offset + 1];
                        data[offset + 1] = rawData[offset + 2];
                        data[offset + 2] = rawData[offset + 3];
                        data[offset + 3] = rawData[offset];
                    }
                    return data;
                }
                default:
                    Debug.Log($"Getting the raw data on an unsupported texture format '{UnityTexture.format}'.");
                    // todo
                    break;
            }
            return data;
        }
    }
}