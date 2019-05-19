using System;
using System.IO;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Common
{
    public static class StreamExtensions
    {
        public static byte[] ReadToEnd(this Stream stream)
        {
            var buffer = new byte[1024];
            var result = new byte[Math.Max(stream.Length, 1024)];
            var resultPos = 0;
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            while (bytesRead > 0)
            {
                result = AdjustArray(result, resultPos + bytesRead);
                Array.Copy(buffer, 0, result, resultPos, bytesRead);
                resultPos += bytesRead;
                bytesRead = stream.Read(buffer, 0, buffer.Length);
            }
            if (result.Length != resultPos)
            {
                var shrunkenResult = new byte[resultPos];
                Array.Copy(result, 0, shrunkenResult, 0, resultPos);
                return shrunkenResult;
            }
            return result;
        }

        private static byte[] AdjustArray(byte[] array, int requiredLength)
        {
            if (array.Length >= requiredLength)
                return array;
            var newArray = new byte[array.Length * 2];
            Array.Copy(array, 0, newArray, 0, array.Length);
            return newArray;
        }
    }
}