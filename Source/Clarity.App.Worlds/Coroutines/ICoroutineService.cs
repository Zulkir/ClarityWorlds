using System;
using Clarity.Common.Infra.AwaitableCoroutines;

namespace Clarity.App.Worlds.Coroutines
{
    public interface ICoroutineService
    {
        IAwcScriptRuntime Runtime { get; }

        AwcAwaiter<AwcWaitUpdatesTask> WaitUpdates(int numUpdates);
        AwcAwaiter<AwcWaitSecondsTask> WaitSeconds(float seconds);
        AwcAwaiter<AwcWaitConditionTask> WaitCondition(Func<bool> condition);
    }
}