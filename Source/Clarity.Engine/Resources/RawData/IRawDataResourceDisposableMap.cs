using System;
using Clarity.Common.CodingUtilities.Unmanaged;

namespace Clarity.Engine.Resources.RawData
{
    public interface IRawDataResourceDisposableMap : IAutoDisposable
    {
        IntPtr Ptr { get; }
    }
}