using System;
using Clarity.Common.CodingUtilities.Unmanaged;
using Clarity.Common.Numericals;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Objects.Caching;
using ObjectGL.Api.Objects.Resources.Images;
using OpenTK.Graphics.OpenGL;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class CgMovieInstanceCache : AutoDisposableBase, ICache
    {
        private readonly IGraphicsInfra infra;
        private readonly IMoviePlayback moviePlayback;
        private readonly ITexture2D glTexture;
        private double currentTimeStamp;

        public CgMovieInstanceCache(IGraphicsInfra infra, IMoviePlayback moviePlayback)
        {
            this.infra = infra;
            this.moviePlayback = moviePlayback;
            var w = moviePlayback.Movie.Width;
            var h = moviePlayback.Movie.Height;
            var mipCount = GraphicsHelper.TextureMipCount(w, h);
            glTexture = infra.GlContext.Create.Texture2D(w, h, mipCount, Format.Rgba8);
            currentTimeStamp = -1;
        }

        protected override void Dispose(bool explicitly)
        {
            infra.MainThreadDisposer.Add(glTexture);
        }

        public void OnMasterEvent(object eventArgs)
        {
        }

        public unsafe ITexture2D GetGlTexture2D()
        {
            if (moviePlayback.FrameRawRgba != null)
            {
                if (currentTimeStamp != moviePlayback.FrameTimestamp)
                {
                    fixed (byte* pPreviousFrame = moviePlayback.FrameRawRgba)
                    {
                        glTexture.SetData(0, (IntPtr)pPreviousFrame, FormatColor.Rgba, FormatType.UnsignedByte);
                        infra.GlContext.GL.GenerateMipmap((int)All.Texture2D);
                        currentTimeStamp = moviePlayback.FrameTimestamp;
                    }
                }
            }
            else
            {
                if (currentTimeStamp != -1)
                {
                    var zeroes = new byte[GraphicsHelper.AlignedSize(moviePlayback.Movie.Width, moviePlayback.Movie.Height)];
                    fixed (byte* pPreviousFrame = zeroes)
                    {
                        glTexture.SetData(0, (IntPtr)pPreviousFrame, FormatColor.Rgba, FormatType.UnsignedByte);
                        infra.GlContext.GL.GenerateMipmap((int)All.Texture2D);
                        currentTimeStamp = -1;
                    }
                }
            }
            return glTexture;
        }
    }
}
