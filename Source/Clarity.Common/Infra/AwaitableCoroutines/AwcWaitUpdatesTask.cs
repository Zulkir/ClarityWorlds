namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public struct AwcWaitUpdatesTask : IAwcTask
    {
        private readonly IAwcScriptRuntime runtime;
        private readonly int targetTotalFrames;

        public bool IsCompleted => runtime.TotalFrames >= targetTotalFrames;

        public AwcWaitUpdatesTask(IAwcScriptRuntime runtime, int targetTotalFrames)
        {
            this.runtime = runtime;
            this.targetTotalFrames = targetTotalFrames;
        }
    }
}