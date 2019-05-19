using System;

namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public struct AwcWaitConditionTask : IAwcTask
    {
        private readonly Func<bool> condition;

        public AwcWaitConditionTask(Func<bool> condition)
        {
            this.condition = condition;
        }

        public bool IsCompleted => condition();
    }
}