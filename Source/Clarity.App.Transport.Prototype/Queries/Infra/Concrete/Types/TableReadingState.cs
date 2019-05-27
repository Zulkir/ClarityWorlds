using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types
{
    public struct TableReadingState
    {
        public IDataTableState State;
        public IDataLogEntry Entry;

        public TableReadingState(IDataTableState state, IDataLogEntry entry)
        {
            State = state;
            Entry = entry;
        }
    }
}