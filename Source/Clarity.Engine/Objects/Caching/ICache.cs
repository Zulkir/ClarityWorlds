using System;

namespace Clarity.Engine.Objects.Caching
{
    public interface ICache : IDisposable
    {
        void OnMasterEvent(object eventArgs);
    }
}