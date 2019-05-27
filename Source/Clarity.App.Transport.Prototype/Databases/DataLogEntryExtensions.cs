namespace Clarity.App.Transport.Prototype.Databases
{
    public static class DataLogEntryExtensions
    {
        public static IDataLogEntry WithOrdinal(this IDataLogEntry entry, int ordinalDisambiguator)
        {
            entry.OrdinalDisambiguator = ordinalDisambiguator;
            return entry;
        }

        public static void Apply(this IDataLogEntry entry, IMutableDataBaseState dataBaseState) => 
            entry.Apply(dataBaseState.GetTableState(entry.Table));

        public static void Undo(this IDataLogEntry entry, IMutableDataBaseState dataBaseState) => 
            entry.Undo(dataBaseState.GetTableState(entry.Table));

        public static void MakeUndoable(this IDataLogEntry entry, IMutableDataBaseState dataBaseState) =>
            entry.MakeUndoable(dataBaseState.GetTableState(entry.Table));
    }
}