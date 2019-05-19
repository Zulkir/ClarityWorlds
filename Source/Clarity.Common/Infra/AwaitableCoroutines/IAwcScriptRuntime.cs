namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public interface IAwcScriptRuntime
    {
        int TotalFrames { get; }
        float TotalSeconds { get; }

        IAwcTaskQueue<T> GetQueue<T>() where T : IAwcTask;

        void OnUpdate(float totalSeconds);
    }
}