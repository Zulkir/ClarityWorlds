using System.Collections.Generic;
using System.Linq;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class DataBase : IDataBase
    {
        public IReadOnlyList<IDataTable> Tables { get; }
        private readonly Dictionary<string, IDataTable> tablesByName;

        public bool HasTableWithName(string name) => tablesByName.ContainsKey(name);
        public IDataTable GetTableByName(string name) => tablesByName[name];

        public DataBase(IReadOnlyList<IDataTable> tables)
        {
            Tables = tables;
            tablesByName = tables.ToDictionary(x => x.Name);
        }
    }
}