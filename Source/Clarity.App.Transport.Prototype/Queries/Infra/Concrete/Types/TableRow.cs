using System;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types
{
    public struct TableRow : IEquatable<TableRow>
    {
        public IDataTableState TableState;
        public int Key;

        public TableRow(IDataTableState table, int key)
        {
            TableState = table;
            Key = key;
        }

        public bool Equals(TableRow other) => Equals(TableState, other.TableState) && Key == other.Key;
        public override bool Equals(object obj) => obj is TableRow && Equals((TableRow)obj);
        public override int GetHashCode() => (TableState.GetHashCode() * 397) ^ Key;
    }
}