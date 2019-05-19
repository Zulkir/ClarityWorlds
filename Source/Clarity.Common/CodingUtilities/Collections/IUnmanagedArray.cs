using System;
using Clarity.Common.CodingUtilities.Unmanaged;

namespace Clarity.Common.CodingUtilities.Collections
{
    public interface IUnmanagedArray : IAutoDisposable
    {
        IntPtr Data { get; }
        int Size { get; }
    }
}