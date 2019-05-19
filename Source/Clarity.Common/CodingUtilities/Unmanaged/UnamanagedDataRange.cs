using System;

namespace Clarity.Common.CodingUtilities.Unmanaged
{
    public struct UnamanagedDataRange
    {
        public IntPtr Start;
        public int Length;

        public UnamanagedDataRange(IntPtr start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}