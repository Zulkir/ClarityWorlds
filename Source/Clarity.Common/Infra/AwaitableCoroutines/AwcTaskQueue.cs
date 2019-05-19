using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Pools;

namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public class AwcTaskQueue<T> : IAwcTaskQueue<T>
        where T : IAwcTask
    {
        private struct TaskWithContinuation
        {
            public T Task;
            public Action Continuation;
        }

        private readonly Dictionary<int, TaskWithContinuation> activeTasks;
        private readonly List<KeyValuePair<int, TaskWithContinuation>> activeTasksCopy;
        private readonly Dictionary<int, T> pendingTasks;
        private readonly IntegerPool integerPool;

        public AwcTaskQueue()
        {
            activeTasks = new Dictionary<int, TaskWithContinuation>();
            activeTasksCopy = new List<KeyValuePair<int, TaskWithContinuation>>();
            pendingTasks = new Dictionary<int, T>();
            integerPool = new IntegerPool();
        }

        public void Update()
        {
            activeTasksCopy.Clear();
            activeTasksCopy.AddRange(activeTasks);

            foreach (var kvp in activeTasksCopy)
            {
                var taskWithContinuation = kvp.Value;
                if (!taskWithContinuation.Task.IsCompleted)
                    continue;

                taskWithContinuation.Continuation();
                var index = kvp.Key;
                activeTasks.Remove(index);
                integerPool.Return(index);
            }
        }

        public void AddPending(T task, out int index)
        {
            index = integerPool.Allocate();
            pendingTasks.Add(index, task);
        }

        public void Activate(int index, Action continuation)
        {
            var task = pendingTasks[index];
            pendingTasks.Remove(index);
            activeTasks.Add(index, new TaskWithContinuation
            {
                Task = task,
                Continuation = continuation
            });
        }
    }
}