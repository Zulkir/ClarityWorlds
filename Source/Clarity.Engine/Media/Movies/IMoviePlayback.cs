using Clarity.Engine.Media.Images;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Movies
{
    public interface IMoviePlayback : IResource
    {
        IMovie Movie { get; }
        MoviePlaybackState State { get; }

        IImage FrameImage { get; }
        byte[] FrameRawRgba { get; }
        double FrameTimestamp { get; }

        double GetVideoSpeed();
        void Play();
        void Pause();
        void SeekToTimestamp(double timestamp);
        void GoToStart();
        void PlayFaster();
        void PlaySlower();
        void ReverseDirection();
        void UpdatePlayStatus();
        void GoToEnd();
        void OnUpdate(FrameTime frameTime);
    }
}