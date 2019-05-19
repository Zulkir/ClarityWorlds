using System;
using System.Collections.Concurrent;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class MainThreadDisposer : IMainThreadDisposer
    {
        private readonly ConcurrentBag<IDisposable> deathBag;

        public bool FinishedWorking { get; set; }

        public MainThreadDisposer()
        {
            deathBag = new ConcurrentBag<IDisposable>();
        }

        public void Add(IDisposable obj)
        {
            if (!FinishedWorking && obj != null)
                deathBag.Add(obj);
        }

        public void DisposeOfAll()
        {
            while (deathBag.TryTake(out var obj))
                obj.Dispose();
        }
    }
}