using System;

namespace Clarity.Common.CodingUtilities.Exceptions
{
    public class DataLoadException : Exception
    {
        public DataLoadException(ErrorInfo error)
         : base(error.Message, error.Exception)
        {
        }
    }
}