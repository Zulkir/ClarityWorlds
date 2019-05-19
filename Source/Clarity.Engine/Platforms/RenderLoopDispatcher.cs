using System;

namespace Clarity.Engine.Platforms
{
    public class RenderLoopDispatcher : IRenderLoopDispatcher
    {
        public float CurrentTimestamp { get; private set; }

        public event Action<FrameTime> BeforeAll;
        public event Action<FrameTime> BeforeUpdate;
        public event Action<FrameTime> Update;
        public event Action<FrameTime> AfterUpdate;
        public event Action<FrameTime> BeforeRender;
        public event Action<FrameTime> Render;
        public event Action<FrameTime> AfterRender;
        public event Action<FrameTime> AfterAll;
        public event Action Closing;

        public void OnLoop(FrameTime frameTime)
        {
            CurrentTimestamp = frameTime.TotalSeconds;

            BeforeAll?.Invoke(frameTime);

            BeforeUpdate?.Invoke(frameTime);
            Update?.Invoke(frameTime);
            AfterUpdate?.Invoke(frameTime);

            BeforeRender?.Invoke(frameTime);
            Render?.Invoke(frameTime);
            AfterRender?.Invoke(frameTime);

            AfterAll?.Invoke(frameTime);
        }

        public void OnClosing()
        {
            Closing?.Invoke();
        }
    }
}