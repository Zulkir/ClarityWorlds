using System;
using Clarity.Common.Numericals;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class PlaybackService : IPlaybackService
    {
        private readonly IDataRetrievalService dataRetrievalService;
        public PlaybackState State { get; set; }
        public double Speed { get; set; }
        public bool Backwards { get; set; }
        public double MinTimestamp { get; set; }
        public double MaxTimestamp { get; set; }
        public double AbsoluteTime { get; set; }

        public double RelativeTime
        {
            get => (AbsoluteTime - MinTimestamp) / (MaxTimestamp - MinTimestamp);
            set => AbsoluteTime = MinTimestamp + value * (MaxTimestamp - MinTimestamp);
        } 

        public PlaybackService(IDataRetrievalService dataRetrievalService)
        {
            this.dataRetrievalService = dataRetrievalService;
            Speed = 100_000;
            MaxTimestamp = Speed;
        }

        public void OnNewFrame(FrameTime frameTime, out bool updated)
        {
            AdjustTimeRange();
            switch (State)
            {
                case PlaybackState.Paused:
                    updated = false;
                    return;
                case PlaybackState.Playing:
                    if (!Backwards)
                    {
                        AbsoluteTime += frameTime.DeltaSeconds * Speed;
                        if (AbsoluteTime > MaxTimestamp)
                        {
                            AbsoluteTime = MaxTimestamp;
                            State = PlaybackState.Paused;
                        }
                        updated = true;
                        return;
                    }
                    else
                    {
                        AbsoluteTime -= frameTime.DeltaSeconds * Speed;
                        if (AbsoluteTime < MinTimestamp)
                        {
                            AbsoluteTime = MinTimestamp;
                            State = PlaybackState.Paused;
                        }
                        updated = true;
                        return;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AdjustTimeRange()
        {
            MinTimestamp = dataRetrievalService.MinOverallTimestamp;
            MaxTimestamp = dataRetrievalService.MaxOverallTimestamp;
            if (AbsoluteTime > MaxTimestamp)
                AbsoluteTime = MaxTimestamp;
            if (AbsoluteTime < MinTimestamp)
                AbsoluteTime = MinTimestamp;
            if (MaxTimestamp - MinTimestamp < MathHelper.Eps5)
                MaxTimestamp = MinTimestamp + MathHelper.Eps5;
        }
    }
}