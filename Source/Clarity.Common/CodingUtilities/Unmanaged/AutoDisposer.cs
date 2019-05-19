using System;

namespace Clarity.Common.CodingUtilities.Unmanaged
{
    public class AutoDisposer : AutoDisposableBase
    {
        private readonly IDisposable master;
        private bool deactivated;

        public AutoDisposer(IDisposable master)
        {
            this.master = master;
        }

        protected override void Dispose(bool explicitly)
        {
            if (!deactivated)
                master.Dispose();
        }

        public void Deactivate()
        {
            deactivated = true;
            GC.SuppressFinalize(this);
        }
    }
}