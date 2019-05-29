using Clarity.App.Worlds.External.Movies;
using Clarity.Engine.Media.Movies;

namespace Clarity.Ext.Video.FFmpeg
{
    public class FFmpegMovieUrlLoader : IMovieUrlLoader
    {
        public IMovie Load(string url)
        {
            return new FFmpegUrlMovie(url);
        }
    }
}