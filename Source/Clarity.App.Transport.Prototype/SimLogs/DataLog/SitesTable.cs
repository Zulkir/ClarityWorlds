using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.SimLogs.DataLog
{
    public class SitesTable : DataTable
    {
        public IDataField IdField { get; }
        public IDataField NameField { get; }

        public SitesTable(int id) : base(id, "Sites", new[]
        {
            new DataFieldDescription(DataFieldType.Int32, "Id"),
            new DataFieldDescription(DataFieldType.String, "Name")
        })
        {
            IdField = Fields[0];
            NameField = Fields[1];
        }
    }
}