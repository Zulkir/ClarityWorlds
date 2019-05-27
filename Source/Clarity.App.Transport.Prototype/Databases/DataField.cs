using System;
using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class DataField : IDataField
    {
        public IDataTable Table { get; }
        public int Index { get; }
        public DataFieldType Type { get; }
        public string Name { get; }

        public DataField(IDataTable table, int index, DataFieldType type, string name)
        {
            Index = index;
            Type = type;
            Name = name;
            Table = table;
        }

        public static IReadOnlyList<DataFieldType> AllTypes { get; } = (DataFieldType[])Enum.GetValues(typeof(DataFieldType));
    }
}