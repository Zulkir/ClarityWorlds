using System;
using Clarity.Common.Infra.AwaitableCoroutines;
using Clarity.Engine.Platforms;

namespace Clarity.App.Worlds.Coroutines
{
    public class CoroutineService : ICoroutineService
    {
        public IAwcScriptRuntime Runtime { get; }

        private readonly IAwcTaskQueue<AwcWaitUpdatesTask> waitUpdatesQueue;
        private readonly IAwcTaskQueue<AwcWaitSecondsTask> waitSecondsQueue;
        private readonly IAwcTaskQueue<AwcWaitConditionTask> waitConditionQueue;

        public CoroutineService(IRenderLoopDispatcher dispatcher)
        {
            Runtime = new AwcScriptRuntime();
            waitUpdatesQueue = Runtime.GetQueue<AwcWaitUpdatesTask>();
            waitSecondsQueue = Runtime.GetQueue<AwcWaitSecondsTask>();
            waitConditionQueue = Runtime.GetQueue<AwcWaitConditionTask>();
            dispatcher.Update += x => Runtime.OnUpdate(x.TotalSeconds);
        }

        public AwcAwaiter<AwcWaitUpdatesTask> WaitUpdates(int numUpdates) => 
            new AwcAwaiter<AwcWaitUpdatesTask>(new AwcWaitUpdatesTask(Runtime, Runtime.TotalFrames + numUpdates), waitUpdatesQueue);

        public AwcAwaiter<AwcWaitSecondsTask> WaitSeconds(float seconds) =>
            new AwcAwaiter<AwcWaitSecondsTask>(new AwcWaitSecondsTask(Runtime, Runtime.TotalSeconds + seconds), waitSecondsQueue);

        public AwcAwaiter<AwcWaitConditionTask> WaitCondition(Func<bool> condition) =>
            new AwcAwaiter<AwcWaitConditionTask>(new AwcWaitConditionTask(condition), waitConditionQueue);
    }
}