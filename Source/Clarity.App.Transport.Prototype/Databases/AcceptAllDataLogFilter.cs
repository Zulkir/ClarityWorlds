namespace Clarity.App.Transport.Prototype.Databases
{
    public class AcceptAllDataLogFilter : IDataLogFilter
    {
        public bool AcceptsTable(IDataTable table) => true;
        public bool AcceptsField(IDataField field) => true;
        public bool AcceptsEntry(IDataLogEntry entry) => true;

        public static AcceptAllDataLogFilter Singleton { get; } = new AcceptAllDataLogFilter();
    }
}