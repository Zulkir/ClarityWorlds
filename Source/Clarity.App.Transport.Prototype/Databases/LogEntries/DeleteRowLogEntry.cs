using System.Collections.Generic;
using JetBrains.Annotations;

namespace Clarity.App.Transport.Prototype.Databases.LogEntries
{
    public class DeleteRowLogEntry : IDataLogEntry
    {
        public double Timestamp { get; }
        public int OrdinalDisambiguator { get; set; }
        public IDataTable Table { get; }
        public IDataField Field => null;
        public int RowKey { get; }
        [CanBeNull] public IReadOnlyList<object> Values => values;

        private object[] values;

        public DeleteRowLogEntry(double timestamp, IDataTable table, int rowKey)
        {
            Timestamp = timestamp;
            Table = table;
            RowKey = rowKey;
        }

        public void MakeUndoable(IDataTableState tableStateRightBefore)
        {
            values = values ?? new object[Table.Fields.Count];
            for (var i = 0; i < values.Length; i++)
                values[i] = tableStateRightBefore.GetValueAbstract(RowKey, i);
        }

        public void Apply(IMutableDataTableState tableState)
        {
            tableState.DeleteRow(RowKey);
        }

        public void Undo(IMutableDataTableState tableState)
        {
            tableState.InsertNewRow(RowKey);
            for (var i = 0; i < Values.Count; i++)
                tableState.SetValueAbstract(RowKey, i, Values[i]);
        }

        public override string ToString() => $"{Timestamp}: delete {Table.Name}[{RowKey}]";
    }
}