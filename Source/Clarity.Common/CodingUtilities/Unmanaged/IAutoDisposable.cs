using System;

namespace Clarity.Common.CodingUtilities.Unmanaged
{
    public interface IAutoDisposable : IDisposable
    {
        bool IsDisposed { get; }
    }
}