using Clarity.Engine.Media.Movies;

namespace Clarity.App.Worlds.External.Movies
{
    public interface IMovieUrlLoader
    {
        IMovie Load(string url);
    }
}