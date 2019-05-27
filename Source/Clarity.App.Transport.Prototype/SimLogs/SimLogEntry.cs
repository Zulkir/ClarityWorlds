namespace Clarity.App.Transport.Prototype.SimLogs
{
    public struct SimLogEntry
    {
        public SimLogEntryHeader Header;
        public string BodyStr;

        public override string ToString() => $"{Header} {BodyStr}";

        public static SimLogEntry Parse(string str)
        {
            return new SimLogEntry
            {
                Header = SimLogEntryHeader.Parse(str),
                BodyStr = str.Substring(39)
            };
        }
    }
}