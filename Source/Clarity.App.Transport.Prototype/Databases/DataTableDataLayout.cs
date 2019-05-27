using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class DataTableDataLayout : IDataTableDataLayout
    {
        public IReadOnlyList<DataTableDataLayoutElement> Elements { get; }
        private readonly int[] strides;

        public DataTableDataLayout(IReadOnlyList<IDataField> fields)
        {
            var elements = new List<DataTableDataLayoutElement>();
            strides = new int[DataField.AllTypes.Count];
            foreach (var field in fields)
            {
                ref var stride = ref strides[(int)field.Type];
                elements.Add(new DataTableDataLayoutElement(field.Type, stride));
                stride++;
            }
            Elements = elements.ToArray();
        }

        public int GetStrideForType(DataFieldType fieldType) => strides[(int)fieldType];
    }
}