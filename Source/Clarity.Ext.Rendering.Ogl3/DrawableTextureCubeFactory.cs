using System;
using Clarity.Engine.Media.Skyboxes;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class DrawableTextureCubeFactory : IDrawableTextureCubeFactory
    {
        private readonly IContext glContext;

        public DrawableTextureCubeFactory(IContext glContext)
        {
            this.glContext = glContext;
        }

        public unsafe IDrawableTextureCube Create(ISkybox clTexture)
        {
            var skyboxTexture = glContext.Create.TextureCubemap(clTexture.Width, 1, Format.Rgba8);
            for (int i = 0; i < FacesInOglOrder.Length; i++)
            {
                var data = clTexture.GetRawData(FacesInOglOrder[i]);
                fixed (byte* pData = data)
                    skyboxTexture.SetData(0, i, (IntPtr)pData, FormatColor.Bgra, FormatType.UnsignedByte);
            }
            return new DrawableTextureCube(skyboxTexture);
        }

        private static readonly SkyboxFace[] FacesInOglOrder = 
        {
            SkyboxFace.Right,
            SkyboxFace.Left,
            SkyboxFace.Top,
            SkyboxFace.Bottom,
            SkyboxFace.Back,
            SkyboxFace.Front
        };
    }
}