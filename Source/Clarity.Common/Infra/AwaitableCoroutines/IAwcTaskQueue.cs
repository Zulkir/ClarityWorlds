using System;

namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public interface IAwcTaskQueue
    {
        void Update();
    }

    public interface IAwcTaskQueue<in T> : IAwcTaskQueue
        where T : IAwcTask
    {
        void AddPending(T task, out int index);
        void Activate(int index, Action continuation);
    }
}