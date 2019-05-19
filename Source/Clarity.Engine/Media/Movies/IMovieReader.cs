using System;
using System.Collections.Generic;

namespace Clarity.Engine.Media.Movies
{
    public interface IMovieReader : IDisposable
    {
        int Width { get; }
        int Height { get; }
        double Duration { get; }

        Queue<MovieFrame> FrameQueue { get; }
        Queue<MovieAudioFrame> AudioQueue { get; }

        bool ReadNextPacket();
        void SeekToTimestamp(double videoStartFactor);
    }
}