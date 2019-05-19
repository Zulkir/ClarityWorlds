using System;
using Clarity.Common.CodingUtilities.Await;

namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public struct AwcAwaiter<T> : IAwaiter
        where T : IAwcTask
    {
        private readonly T task;
        private readonly IAwcTaskQueue<T> queue;
        private readonly int index;

        public AwcAwaiter(T task, IAwcTaskQueue<T> queue)
        {
            this.task = task;
            this.queue = queue;
            queue.AddPending(task, out index);
        }

        public AwcAwaiter<T> GetAwaiter() => this;
        public void GetResult() { }
        public bool IsCompleted => task.IsCompleted;
        public void OnCompleted(Action continuation) => queue.Activate(index, continuation);
    }
}