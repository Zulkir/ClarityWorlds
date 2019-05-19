using System;
using System.Text;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Common
{
    public static class ExceptionExtensions
    {
        public static string GetCompleteMessage(this Exception ex)
        {
            var builder = new StringBuilder();
            var currentEx = ex;
            while (currentEx != null)
            {
                builder.AppendLine(currentEx.Message);
                builder.AppendLine();
                currentEx = currentEx.InnerException;
            }
            builder.AppendLine(ex.StackTrace);
            return builder.ToString();
        }
    }
}