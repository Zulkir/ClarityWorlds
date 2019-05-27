using System.Collections;
using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class DataLogEnumerable : IEnumerable<DataLogReadingState>
    {
        private readonly IDataBaseState initialState;
        private readonly IEnumerable<IDataLogEntry> entries;

        public DataLogEnumerable(IDataBaseState initialState, IEnumerable<IDataLogEntry> entries)
        {
            this.initialState = initialState;
            this.entries = entries;
        }

        public IEnumerator<DataLogReadingState> GetEnumerator() => new DataLogEnumerator(initialState, entries);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}