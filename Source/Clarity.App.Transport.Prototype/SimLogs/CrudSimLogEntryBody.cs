namespace Clarity.App.Transport.Prototype.SimLogs
{
    public struct CrudSimLogEntryBody
    {
        public string Table;
        public string FromSite;
        public string ToSite;
        public int TupleId;
        public int TupleSize;
        public byte TupleSecurity;
        public byte TupleUrgency;
        public byte TupleReplication;
        public byte TupleFrequency;
        public string Reserved0;
        public string Reserved1;
        public string Reserved2;
        public long CommunicationTime;
        public int CommunicationCost;
        public string Reserved3;

        public override string ToString() =>
            $"{Table,4} {FromSite,4} {ToSite,4} {TupleId,6} {TupleSize,6} {TupleSecurity,2} {TupleUrgency,2} {TupleReplication,2} {TupleFrequency,2} {Reserved0,20} {Reserved1,20} {Reserved2,20} {CommunicationTime,12} {CommunicationCost,6} {Reserved3,4}";

        public static CrudSimLogEntryBody Parse(string str)
        {
            return new CrudSimLogEntryBody
            {
                Table = str.Substring(0, 4).Trim(),
                FromSite = str.Substring(5, 4).Trim(),
                ToSite = str.Substring(10, 4).Trim(),
                TupleId = int.Parse(str.Substring(15, 6)),
                TupleSize = int.Parse(str.Substring(22, 6)),
                TupleSecurity = byte.Parse(str.Substring(29, 2)),
                TupleUrgency = byte.Parse(str.Substring(32, 2)),
                TupleReplication = byte.Parse(str.Substring(35, 2)),
                TupleFrequency = byte.Parse(str.Substring(38, 2)),
                Reserved0 = str.Substring(41, 20),
                Reserved1 = str.Substring(62, 20),
                Reserved2 = str.Substring(83, 20),
                CommunicationTime = long.Parse(str.Substring(104, 12)),
                CommunicationCost = int.Parse(str.Substring(117, 6)),
                Reserved3 = str.Substring(124, 4)
            };
        }
    }
}