using System;
using JetBrains.Annotations;

namespace Clarity.Common.CodingUtilities
{
    public struct ErrorInfo
    {
        [NotNull]
        public string Message { get; }
        [CanBeNull]
        public Exception Exception { get; }

        public ErrorInfo(Exception exception)
        {
            Message = exception.Message;
            Exception = exception;
        }

        public ErrorInfo(string message, Exception exception = null)
        {
            Message = message;
            Exception = exception;
        }
    }
}