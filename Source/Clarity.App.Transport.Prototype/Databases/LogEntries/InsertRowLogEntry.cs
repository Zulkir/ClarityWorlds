namespace Clarity.App.Transport.Prototype.Databases.LogEntries
{
    public class InsertRowLogEntry : IDataLogEntry
    {
        public double Timestamp { get; }
        public int OrdinalDisambiguator { get; set; }
        public IDataTable Table { get; }
        public IDataField Field => null;
        public int RowKey { get; }

        public InsertRowLogEntry(double timestamp, IDataTable table, int rowKey)
        {
            Timestamp = timestamp;
            Table = table;
            RowKey = rowKey;
        }

        public void MakeUndoable(IDataTableState tableStateRightBefore) {  }

        public void Apply(IMutableDataTableState tableState)
        {
            tableState.InsertNewRow(RowKey);
        }

        public void Undo(IMutableDataTableState tableState)
        {
            tableState.DeleteRow(RowKey);
        }

        public override string ToString() => $"{Timestamp}: insert {Table.Name}[{RowKey}]";
    }
}