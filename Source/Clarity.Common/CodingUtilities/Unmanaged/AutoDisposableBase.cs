using System;

namespace Clarity.Common.CodingUtilities.Unmanaged
{
    public abstract class AutoDisposableBase : IAutoDisposable
    {
        private volatile bool disposeCalled;

        public bool IsDisposed => disposeCalled;

        ~AutoDisposableBase()
        {
            if (disposeCalled)
                return;
            disposeCalled = true;
            Dispose(false);
        }

        public void Dispose()
        {
            if (disposeCalled)
                return;
            disposeCalled = true;
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected abstract void Dispose(bool explicitly);
    }
}