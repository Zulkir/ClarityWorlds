using System.Diagnostics;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class FrameTimeMeasurer : IFrameTimeMeasurer
    {
        private readonly Stopwatch stopwatch;
        private float prevTotalSeconds;

        public FrameTimeMeasurer()
        {
            stopwatch = new Stopwatch();
        }

        public FrameTime MeasureTime()
        {
            if (prevTotalSeconds == 0f)
                stopwatch.Start();
            var totalSeconds = (float)stopwatch.Elapsed.TotalSeconds;
            var deltaSeconds = totalSeconds - prevTotalSeconds;
            prevTotalSeconds = totalSeconds;
            return new FrameTime(totalSeconds, deltaSeconds);
        }
    }
}