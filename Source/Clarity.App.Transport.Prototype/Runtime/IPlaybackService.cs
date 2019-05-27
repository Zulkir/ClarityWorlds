using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IPlaybackService
    {
        PlaybackState State { get; set; }
        double Speed { get; set; }
        bool Backwards { get; set; }
        double MinTimestamp { get; set; }
        double MaxTimestamp { get; set; }
        double AbsoluteTime { get; set; }
        double RelativeTime { get; set; }
        void OnNewFrame(FrameTime frameTime, out bool updated);
    }
}