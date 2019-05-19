using System;
using Clarity.App.Transport.Prototype.Simulation;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype
{
    public interface IPlayback
    {
        ISimState SimState { get; }

        PlaybackState State { get; set; }
        double Speed { get; set; }
        bool Backwards { get; set; }

        double AbsoluteTime { get; }
        double RelativeTime { get; }
        long LastEntryIndex { get; }

        string SelectedSite { get; set; }
        SimPackage SelectedPackage { get; set; }

        void SeekRelative(double relativeTime);

        void OpenFile(string path);

        event Action<FrameTime> Updated;
    }
}