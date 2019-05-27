namespace Clarity.App.Transport.Prototype.Databases
{
    public struct DataFieldDescription
    {
        public DataFieldType Type;
        public string Name;

        public DataFieldDescription(DataFieldType type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}