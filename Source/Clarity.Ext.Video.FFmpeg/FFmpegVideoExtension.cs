using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Video.FFmpeg
{
    public class FFmpegVideoExtension : IExtension
    {
        public string Name => "Video.FFmpeg";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IAssetLoader>().To<FFmpegMovieLoader>();
            di.Bind<IMoviePlayer>().To<FFmpegMoviePlayer>();
            di.Bind<FFmpegInitializer>().AsLastChoice.To<FFmpegInitializer>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}