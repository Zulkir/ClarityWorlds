using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases.LogEntries;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class DataLogBuilder
    {
        private readonly List<IDataLogEntry> entries;
        private readonly Dictionary<IDataTable, MutableDataTableState> tableStates;

        public DataLogBuilder()
        {
            entries = new List<IDataLogEntry>();
            tableStates = new Dictionary<IDataTable, MutableDataTableState>();
        }

        public void InsertRow(double timestamp, IDataTable table, int rowKey) => 
            AddEntry(new InsertRowLogEntry(timestamp, table, rowKey));

        public void DeleteRow(double timestamp, IDataTable table, int rowKey) => 
            AddEntry(new DeleteRowLogEntry(timestamp, table, rowKey));

        public void SetValue<T>(double timestamp, IDataTable table, int rowKey, IDataField field, T value) => 
            AddEntry(new SetValueLogEntry<T>(timestamp, table, rowKey, field, value));

        private void AddEntry(IDataLogEntry entry)
        {
            var insertIndex = GetInsertIndex(entry.Timestamp, 0, entries.Count);
            RewindToBefore(insertIndex);
            entries.Insert(insertIndex, entry);
            AdjustUndoablesFrom(insertIndex);
        }

        private int GetInsertIndex(double timestamp, int rangeStart, int rangeEnd)
        {
            if (rangeEnd == rangeStart)
                return rangeStart;
            var middleIndex = (rangeStart + rangeEnd) / 2;
            var middleTimestamp = entries[middleIndex].Timestamp;
            return middleTimestamp > timestamp 
                ? GetInsertIndex(timestamp, rangeStart, middleIndex) 
                : GetInsertIndex(timestamp, middleIndex, rangeEnd);
        }

        private void RewindToBefore(int index)
        {
            for (var i = entries.Count - 1; i >= index; i--)
                entries[i].Undo(tableStates[entries[i].Table]);
        }

        private void AdjustUndoablesFrom(int index)
        {
            for (var i = index; i < entries.Count; i++)
            {
                var entry = entries[i];
                var state = tableStates[entry.Table];
                entry.MakeUndoable(state);
                entry.Apply(state);
            }
        }
    }
}