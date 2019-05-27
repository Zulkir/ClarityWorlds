using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.SimLogs.DataLog
{
    public class MessagesTable : DataTable
    {
        public IDataField SequenceField { get; }
        public IDataField CodeField { get; }
        public IDataField AppIdField { get; }
        public IDataField FromField { get; }
        public IDataField ToField { get; }
        public IDataField SizeField { get; }
        public IDataField TimeField { get; }
        public IDataField CostField { get; }

        public MessagesTable(int id) : base(id, "Messages", new []
        {
            new DataFieldDescription(DataFieldType.Int32, "Sequence"),
            new DataFieldDescription(DataFieldType.Int32, "Code"),
            new DataFieldDescription(DataFieldType.Int32, "AppId"),
            new DataFieldDescription(DataFieldType.Int32, "From"),
            new DataFieldDescription(DataFieldType.Int32, "To"),
            new DataFieldDescription(DataFieldType.Int32, "Size"),
            new DataFieldDescription(DataFieldType.Float, "Time"),
            new DataFieldDescription(DataFieldType.Int32, "Cost")
        })
        {
            SequenceField = Fields[0];
            CodeField = Fields[1];
            AppIdField = Fields[2];
            FromField = Fields[3];
            ToField = Fields[4];
            SizeField = Fields[5];
            TimeField = Fields[6];
            CostField = Fields[7];
        }
    }
}