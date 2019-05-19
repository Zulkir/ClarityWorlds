using Clarity.Engine.Media.Movies;

namespace Clarity.Ext.Video.FFmpeg
{
    public class FFmpegMoviePlayer : IMoviePlayer
    {
        public FFmpegMoviePlayer(FFmpegInitializer initializer)
        {
            initializer.EnsureInitialized();
        }

        public IMoviePlayback CreatePlayback(IMovie movie) => new StandardMoviePlayback(movie);
    }
}