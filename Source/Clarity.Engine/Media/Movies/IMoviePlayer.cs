namespace Clarity.Engine.Media.Movies
{
    public interface IMoviePlayer
    {
        IMoviePlayback CreatePlayback(IMovie movie);
    }
}