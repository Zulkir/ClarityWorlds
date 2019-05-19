using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Movies
{
    public interface IMovie : IResource
    {
        int Width { get; }
        int Height { get; }
        double Duration { get; }
        bool HasTransparency { get; }

        IMovieReader Read();
    }
}