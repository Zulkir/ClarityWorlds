using System.Collections.Generic;
using System.Linq;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class DataTable : IDataTable
    {
        public int Id { get; }
        public string Name { get; }
        public IReadOnlyList<IDataField> Fields { get; }
        private readonly Dictionary<string, IDataField> fieldsByName;

        public DataTable(int id, string name, IEnumerable<DataFieldDescription> fieldDescriptions)
        {
            Id = id;
            Name = name;
            Fields = fieldDescriptions.Select((x, i) => new DataField(this, i, x.Type, x.Name)).ToArray();
            fieldsByName = Fields.ToDictionary(x => x.Name);
        }

        public bool TryGetField(string name, out IDataField field) => fieldsByName.TryGetValue(name, out field);
    }
}