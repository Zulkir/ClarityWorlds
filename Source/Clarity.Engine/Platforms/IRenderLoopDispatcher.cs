using System;

namespace Clarity.Engine.Platforms
{
    public interface IRenderLoopDispatcher
    {
        float CurrentTimestamp { get; }

        event Action<FrameTime> BeforeAll;
        event Action<FrameTime> BeforeUpdate;
        event Action<FrameTime> Update;
        event Action<FrameTime> AfterUpdate;
        event Action<FrameTime> BeforeRender;
        event Action<FrameTime> Render;
        event Action<FrameTime> AfterRender;
        event Action<FrameTime> AfterAll;
        event Action Closing;

        void OnLoop(FrameTime frameTime);
        void OnClosing();
    }
}