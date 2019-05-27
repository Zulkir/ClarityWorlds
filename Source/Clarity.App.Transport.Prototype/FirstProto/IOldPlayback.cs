using System;
using Clarity.App.Transport.Prototype.FirstProto.Simulation;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.FirstProto
{
    public interface IOldPlayback
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