using System;
using System.Threading;

namespace Clarity.Common.CodingUtilities.Sugar.Wrappers.Threading
{
    public class ReaderWriterLockSlimSugar : IDisposable
    {
        private class ReleaseRead : IDisposable
        {
            private readonly ReaderWriterLockSlim internalLock;
            public ReleaseRead(ReaderWriterLockSlim internalLock) { this.internalLock = internalLock; }
            public void Dispose() => internalLock.ExitReadLock();
        }

        private class ReleaseWrite : IDisposable
        {
            private readonly ReaderWriterLockSlim internalLock;
            public ReleaseWrite(ReaderWriterLockSlim internalLock) { this.internalLock = internalLock; }
            public void Dispose() => internalLock.ExitWriteLock();
        }

        private readonly ReaderWriterLockSlim internalLock;
        private readonly ReleaseRead releaseRead;
        private readonly ReleaseWrite releaseWrite;

        public ReaderWriterLockSlimSugar(ReaderWriterLockSlim internalLock)
        {
            this.internalLock = internalLock;
            releaseRead = new ReleaseRead(internalLock);
            releaseWrite = new ReleaseWrite(internalLock);
        }

        public void Dispose()
        {
            internalLock?.Dispose();
        }

        public IDisposable LockRead()
        {
            internalLock.EnterReadLock();
            return releaseRead;
        }

        public IDisposable LockWrite()
        {
            internalLock.EnterWriteLock();
            return releaseWrite;
        }
    }
}