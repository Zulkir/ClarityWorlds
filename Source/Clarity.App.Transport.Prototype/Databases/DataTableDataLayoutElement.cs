namespace Clarity.App.Transport.Prototype.Databases
{
    public struct DataTableDataLayoutElement
    {
        public DataFieldType Type;
        public int IndexOffset;

        public DataTableDataLayoutElement(DataFieldType type, int indexOffset)
        {
            Type = type;
            IndexOffset = indexOffset;
        }
    }
}