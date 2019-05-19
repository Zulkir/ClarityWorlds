namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public struct AwcWaitSecondsTask : IAwcTask
    {
        private readonly IAwcScriptRuntime runtime;
        private readonly float targetTotalSeconds;

        public bool IsCompleted => runtime.TotalSeconds >= targetTotalSeconds;

        public AwcWaitSecondsTask(IAwcScriptRuntime runtime, float targetTotalSeconds)
        {
            this.runtime = runtime;
            this.targetTotalSeconds = targetTotalSeconds;
        }
    }
}