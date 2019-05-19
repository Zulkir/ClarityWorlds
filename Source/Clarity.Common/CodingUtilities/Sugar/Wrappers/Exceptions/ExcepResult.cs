using System;

namespace Clarity.Common.CodingUtilities.Sugar.Wrappers.Exceptions
{
    public static class ExcepResult
    {
        public static T New<T>(Exception exception)
        {
            throw exception;
        }
    }
}