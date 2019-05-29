using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Resources;

namespace Clarity.Ext.Video.FFmpeg
{
    public class FFmpegUrlMovie : ResourceBase, IMovie
    {
        [TrwSerialize]
        public class FFmpegUrlMovieSource : IResourceSource
        {
            [TrwSerialize]
            public string Url { get; set; }

            public FFmpegUrlMovieSource(string url)
            {
                Url = url;
            }

            public IResource GetResource()
            {
                return new FFmpegUrlMovie(Url);
            }
        }

        public string Url { get; }

        public int Width { get; }
        public int Height { get; }
        public double Duration { get; }

        public bool HasTransparency => false;

        public FFmpegUrlMovie(string url) : base(ResourceVolatility.Stable)
        {
            Url = url;
            using (var reader = Read())
            {
                Width = reader.Width;
                Height = reader.Height;
                Duration = reader.Duration;
            }
            Source = new FFmpegUrlMovieSource(url);
        }

        public IMovieReader Read()
        {
            return new FFmpegMovieReader(Url);
        }
    }
}