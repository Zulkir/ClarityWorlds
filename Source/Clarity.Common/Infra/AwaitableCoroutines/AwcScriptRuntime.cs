using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public class AwcScriptRuntime : IAwcScriptRuntime
    {
        private readonly Dictionary<Type, IAwcTaskQueue> queues;

        private float totalSecondsOffset;

        public float TotalSeconds { get; private set; }
        public int TotalFrames { get; private set; }

        public AwcScriptRuntime()
        {
            queues = new Dictionary<Type, IAwcTaskQueue>();
        }

        public IAwcTaskQueue<T> GetQueue<T>()
            where T : IAwcTask
        {
            return (IAwcTaskQueue<T>)queues.GetOrAdd(typeof(T), t => new AwcTaskQueue<T>());
        }

        public void OnUpdate(float totalSeconds)
        {
            if (totalSecondsOffset == 0f)
                totalSecondsOffset = totalSeconds;

            TotalSeconds = totalSeconds - totalSecondsOffset;
            TotalFrames++;

            foreach (var queue in queues.Values)
                queue.Update();
        }
    }
}