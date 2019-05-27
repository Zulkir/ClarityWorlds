using System;
using Clarity.Common.Numericals;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Objects.Caching;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3.Caches
{
    public class SkyboxCache : ICache
    {
        private static readonly SkyboxFace[] FacesInOglOrder = 
        {
            SkyboxFace.Right,
            SkyboxFace.Left,
            SkyboxFace.Top,
            SkyboxFace.Bottom,
            SkyboxFace.Back,
            SkyboxFace.Front
        };

        private readonly IGraphicsInfra infra;
        private readonly ISkybox skybox;
        private ITextureCubemap glTexture;
        private bool isDirty;

        public SkyboxCache(IGraphicsInfra infra, ISkybox skybox)
        {
            this.skybox = skybox;
            this.infra = infra;
            isDirty = true;
        }

        public void OnMasterEvent(object eventArgs)
        {
            isDirty = true;
        }

        public unsafe ITextureCubemap GetGlTextureCubemap()
        {
            if (!isDirty)
                return glTexture;

            var width = skybox.Width;
            if (glTexture == null || glTexture.Width != width)
            {
                glTexture?.Dispose();
                var mipCount = GraphicsHelper.TextureMipCount(width);
                glTexture = infra.GlContext.Create.TextureCubemap(width, mipCount, Format.Srgb8Alpha8);
            }

            for (var i = 0; i < FacesInOglOrder.Length; i++)
            {
                var data = skybox.GetRawData(FacesInOglOrder[i]);
                fixed (byte* pData = data)
                    glTexture.SetData(0, i, (IntPtr)pData, FormatColor.Bgra, FormatType.UnsignedByte);
            }
            glTexture.GenerateMipmap();
            isDirty = false;

            return glTexture;
        }

        public void Dispose()
        {
            infra.MainThreadDisposer.Add(glTexture);
        }
    }
}