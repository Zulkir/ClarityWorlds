using System;
using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class TableService : ITableService
    {
        private readonly List<IDataTable> tables;
        private readonly Dictionary<string, IDataTable> tablesByName;
        private readonly Stack<int> freedIds;
        private int nextId;

        public TableService()
        {
            tables = new List<IDataTable>();
            tablesByName = new Dictionary<string, IDataTable>();
            freedIds = new Stack<int>();
            nextId = 0;
        }

        public int GenerateId() => 
            freedIds.Count > 0 ? freedIds.Pop() : nextId++;

        public IDataTable CreateTable(string name, IEnumerable<DataFieldDescription> fieldDescriptions)
        {
            if (tablesByName.ContainsKey(name))
                throw new ArgumentException($"A table with name '{name}' is already registered.");
            var id = GenerateId();
            var table = new DataTable(id, name, fieldDescriptions);
            while (tables.Count <= id)
                tables.Add(null);
            tables.Insert(id, table);
            tablesByName.Add(table.Name, table);
            return table;
        }

        public void DropTable(int id)
        {
            var table = tables[id];
            tables[id] = null;
            tablesByName.Remove(table.Name);
            freedIds.Push(id);
        }
    }
}