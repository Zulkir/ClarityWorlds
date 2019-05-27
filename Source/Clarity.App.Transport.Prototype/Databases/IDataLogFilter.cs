namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataLogFilter
    {
        bool AcceptsTable(IDataTable table);
        bool AcceptsField(IDataField field);
        bool AcceptsEntry(IDataLogEntry entry);
    }
}