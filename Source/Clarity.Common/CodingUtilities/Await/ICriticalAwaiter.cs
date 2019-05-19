using System.Runtime.CompilerServices;

namespace Clarity.Common.CodingUtilities.Await
{
    public interface ICriticalAwaiter : IAwaiter, ICriticalNotifyCompletion
    {        
    }

    public interface ICriticalAwaiter<out TResult> : IAwaiter<TResult>, ICriticalNotifyCompletion
    {
        
    }
}