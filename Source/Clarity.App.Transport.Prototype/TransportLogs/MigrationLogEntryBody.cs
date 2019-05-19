namespace Clarity.App.Transport.Prototype.TransportLogs
{
    public struct MigrationLogEntryBody
    {
        public string SourceSite;
        public string TargetSite;
        public int TotalSize;
        public long CommunicationTime;
        public int CommunicationCost;

        public override string ToString() =>
            $"{SourceSite,4} {TargetSite,4} {TotalSize,6} {CommunicationTime,12} {CommunicationCost,6}";

        public static MigrationLogEntryBody Parse(string str)
        {
            return new MigrationLogEntryBody
            {
                SourceSite = str.Substring(0, 4).Trim(),
                TargetSite = str.Substring(5, 4).Trim(),
                TotalSize = int.Parse(str.Substring(10, 6)),
                CommunicationTime = long.Parse(str.Substring(17, 12)),
                CommunicationCost = int.Parse(str.Substring(30, 6))
            };
        }
    }
}