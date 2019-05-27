using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataTableDataLayout
    {
        IReadOnlyList<DataTableDataLayoutElement> Elements { get; }
        int GetStrideForType(DataFieldType fieldType);
    }
}