namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataField
    {
        IDataTable Table { get; }
        int Index { get; }
        DataFieldType Type { get; }
        string Name { get; }
    }
}