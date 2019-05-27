using System;
using Clarity.Common.Infra.AwaitableCoroutines;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;

namespace Clarity.App.Worlds.Coroutines
{
    public class CoroutineService : ICoroutineService
    {
        public IAwcScriptRuntime Runtime { get; }

        private readonly IAwcTaskQueue<AwcWaitUpdatesTask> waitUpdatesQueue;
        private readonly IAwcTaskQueue<AwcWaitSecondsTask> waitSecondsQueue;
        private readonly IAwcTaskQueue<AwcWaitConditionTask> waitConditionQueue;

        public CoroutineService(IEventRoutingService eventRoutingService)
        {
            Runtime = new AwcScriptRuntime();
            waitUpdatesQueue = Runtime.GetQueue<AwcWaitUpdatesTask>();
            waitSecondsQueue = Runtime.GetQueue<AwcWaitSecondsTask>();
            waitConditionQueue = Runtime.GetQueue<AwcWaitConditionTask>();
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(ICoroutineService), nameof(OnNewFrameEvent), OnNewFrameEvent);
        }

        private void OnNewFrameEvent(INewFrameEvent evnt)
        {
            Runtime.OnUpdate(evnt.FrameTime.TotalSeconds);
        }

        public AwcAwaiter<AwcWaitUpdatesTask> WaitUpdates(int numUpdates) => 
            new AwcAwaiter<AwcWaitUpdatesTask>(new AwcWaitUpdatesTask(Runtime, Runtime.TotalFrames + numUpdates), waitUpdatesQueue);

        public AwcAwaiter<AwcWaitSecondsTask> WaitSeconds(float seconds) =>
            new AwcAwaiter<AwcWaitSecondsTask>(new AwcWaitSecondsTask(Runtime, Runtime.TotalSeconds + seconds), waitSecondsQueue);

        public AwcAwaiter<AwcWaitConditionTask> WaitCondition(Func<bool> condition) =>
            new AwcAwaiter<AwcWaitConditionTask>(new AwcWaitConditionTask(condition), waitConditionQueue);
    }
}