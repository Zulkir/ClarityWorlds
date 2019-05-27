using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.App.Transport.Prototype.FirstProto.Simulation;
using Clarity.App.Transport.Prototype.FirstProto.Visualization;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.App.Transport.Prototype.SimLogs;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.FirstProto
{
    public class OldPlayback : IOldPlayback
    {
        private readonly IStateVisualizer stateVisualizer;
        private readonly ISimFrameGenerator frameGenerator;

        private readonly List<ISimFrame> frames;
        private int nextFrameIndex;
        private ISimState simState;

        public PlaybackState State { get; set; }
        public double Speed { get; set; }
        public bool Backwards { get; set; }
        public double AbsoluteTime => simState.Timestamp;
        public double RelativeTime => frames.Count == 0 ? 0 : simState.Timestamp / frames.Last().Timestamp;
        public long LastEntryIndex { get; private set; }
        public string SelectedSite { get; set; }
        public SimPackage SelectedPackage { get; set; }
        public event Action<FrameTime> Updated;

        public ISimState SimState => simState;

        private double LastFrameTimestamp => frames.Count == 0 ? 0 : frames.Last().Timestamp;

        public OldPlayback(IRenderLoopDispatcher renderLoopDispatcher, IStateVisualizer stateVisualizer, ISimFrameGenerator frameGenerator)
        {
            this.stateVisualizer = stateVisualizer;
            this.frameGenerator = frameGenerator;
            simState = new SimState();
            frames = new List<ISimFrame>();
            Speed = 1_000_000;
            renderLoopDispatcher.Update += Update;
        }

        private void Update(FrameTime frameTime)
        {
            if (State != PlaybackState.Paused)
            {
                if (!Backwards)
                {
                    simState.Timestamp += frameTime.DeltaSeconds * Speed;
                    var nextTimestamp = simState.Timestamp;
                    while (nextFrameIndex < frames.Count && frames[nextFrameIndex].Timestamp < nextTimestamp)
                    {
                        var frame = frames[nextFrameIndex];
                        frame.Apply(simState);
                        if (frame.IncedentalEntryIndex.HasValue)
                            LastEntryIndex = frame.IncedentalEntryIndex.Value;
                        nextFrameIndex++;
                    }
                }
                else
                {
                    simState.Timestamp -= frameTime.DeltaSeconds * Speed;
                    var nextTimestamp = simState.Timestamp;
                    nextFrameIndex--;
                    while (nextFrameIndex >= 0 && frames[nextFrameIndex].Timestamp > nextTimestamp)
                    {
                        var frame = frames[nextFrameIndex];
                        frame.Undo(simState);
                        if (frame.IncedentalEntryIndex.HasValue)
                            LastEntryIndex = frame.IncedentalEntryIndex.Value;
                        nextFrameIndex--;
                    }
                    nextFrameIndex++;
                }
            }
            stateVisualizer.UpdateToState(simState);
            Updated?.Invoke(frameTime);
        }

        public void SeekRelative(double relativeTime)
        {
            if (relativeTime == 0)
            {
                simState = new SimState();
                simState.Timestamp = 0;
                nextFrameIndex = 0;
                LastEntryIndex = 0;
            }
            else if (relativeTime == 1)
            {
                simState = new SimState();
                simState.Timestamp = LastFrameTimestamp;
                nextFrameIndex = frames.Count;
                LastEntryIndex = Enumerable.Reverse(frames).Select(x => x.IncedentalEntryIndex)
                                     .FirstOrDefault(x => x.HasValue) ?? 0;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void OpenFile(string filePath)
        {
            var unsortedFrames = new List<ISimFrame>();
            nextFrameIndex = 0;
            simState = new SimState();
            if (Path.GetExtension(filePath) == ".dlog")
            {
                using (var reader = new StreamReader(filePath))
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var entry = SimLogEntry.Parse(line);
                        foreach (var frame in frameGenerator.FromLogEntry(entry))
                            unsortedFrames.Add(frame);
                    }
            }
            else
            {
                using (var stream = File.OpenRead(filePath))
                using (var logReader = new SimLogReader(stream, true))
                    foreach (var entry in logReader.ReadEntries())
                    foreach (var frame in frameGenerator.FromLogEntry(entry))
                        unsortedFrames.Add(frame);
            }
            frames.Clear();
            frames.AddRange(unsortedFrames.OrderBy(x => x.Timestamp));
        }
    }
}