using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataTable
    {
        int Id { get; }
        string Name { get; }
        IReadOnlyList<IDataField> Fields { get; }
        bool TryGetField(string name, out IDataField field);
    }
}