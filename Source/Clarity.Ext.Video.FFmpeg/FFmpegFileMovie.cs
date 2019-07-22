using System;
using System.IO;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Resources;

namespace Clarity.Ext.Video.FFmpeg
{
    public class FFmpegFileMovie : ResourceBase, IMovie
    {
        public int Width { get; }
        public int Height { get; }
        public double Duration { get; }
        private readonly Func<Stream> openFile;

        public bool HasTransparency => false;

        public FFmpegFileMovie(int width, int height, double durationInSeconds, Func<Stream> openFile) 
            : base(ResourceVolatility.Stable)
        {
            Width = width;
            Height = height;
            Duration = durationInSeconds;
            this.openFile = openFile;
        }

        public IMovieReader Read()
        {
            return new FFmpegMovieReader(openFile());
        }
    }
}