using System.Collections;
using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class DataLogEnumerator : IEnumerator<DataLogReadingState>
    {
        private readonly IDataBaseState initialState;
        private readonly IEnumerator<IDataLogEntry> entryEnumerator;
        private IMutableDataBaseState currentState;

        public DataLogReadingState Current => new DataLogReadingState(entryEnumerator.Current, currentState);
        object IEnumerator.Current => Current;

        public DataLogEnumerator(IDataBaseState initialState, IEnumerable<IDataLogEntry> entries)
        {
            this.initialState = initialState;
            entryEnumerator = entries.GetEnumerator();
            currentState = new MutableDataBaseState(initialState.Tables, initialState);
        }

        public void Dispose()
        {
            entryEnumerator?.Dispose();
        }

        public bool MoveNext()
        {
            if (!entryEnumerator.MoveNext())
                return false;
            var entry = entryEnumerator.Current;
            entry.Apply(currentState.GetTableState(entry.Table));
            return true;
        }

        public void Reset()
        {
            currentState = new MutableDataBaseState(initialState.Tables, initialState);
            entryEnumerator.Reset();
        }
    }
}