using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;

namespace Clarity.Ext.Gui.EtoForms.Common
{
    public class NameGenerator : INameGenerator
    {
        private class Counter
        {
            private int internalCounter;
            public int Current { get { return internalCounter; } }
            public Counter(int initialValue = 0) { internalCounter = initialValue; }
            public void Inc() { Interlocked.Increment(ref internalCounter); }
        }

        private readonly ConcurrentDictionary<Type, Counter> counters = new ConcurrentDictionary<Type, Counter>();

        public string GenerateName<T>()
        {
            var counter = counters.GetOrAdd(typeof(T), x => new Counter());
            counter.Inc();
            var number = counter.Current;
            var str = typeof(T).IsInterface
                ? typeof(T).Name.Substring(1)
                : typeof(T).Name;
            return number == 1
                ? str
                : str + number.ToString(CultureInfo.InvariantCulture);
        }
    }
}