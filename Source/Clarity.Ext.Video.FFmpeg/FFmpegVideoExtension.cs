using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.External.Movies;
using Clarity.Common.Infra.DependencyInjection;
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
            di.Bind<IMovieUrlLoader>().To<FFmpegMovieUrlLoader>();
            di.Bind<FFmpegInitializer>().AsLastChoice.To<FFmpegInitializer>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}