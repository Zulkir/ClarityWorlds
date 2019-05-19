using Clarity.Engine.Media.Movies;

namespace Assets.Scripts.Hacks
{
    public class UcCgMoviePlayer : IMoviePlayer
    {
        public IMoviePlayback CreatePlayback(IMovie movie)
        {
            throw new System.NotImplementedException();
        }
    }
}