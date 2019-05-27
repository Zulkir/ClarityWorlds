namespace Clarity.App.Transport.Prototype.Databases
{
    public struct DataLogReadingState
    {
        public IDataLogEntry Entry;
        public IDataBaseState State;

        public DataLogReadingState(IDataLogEntry entry, IDataBaseState state)
        {
            Entry = entry;
            State = state;
        }
    }
}