using System;
using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities.Collections;

namespace Clarity.Common.CodingUtilities.Unmanaged
{
    public class UnmanagedArray : AutoDisposableBase, IUnmanagedArray
    {
        public IntPtr Data { get; }
        public int Size { get; }

        public UnmanagedArray(int size)
        {
            Size = size;
            Data = Marshal.AllocHGlobal(size);
        }

        protected override void Dispose(bool explicitly)
        {
            Marshal.FreeHGlobal(Data);
        }
    }
}