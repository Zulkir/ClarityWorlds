namespace Clarity.Common.Infra.AwaitableCoroutines
{
    public interface IAwcTask
    {
        bool IsCompleted { get; }
    }
}