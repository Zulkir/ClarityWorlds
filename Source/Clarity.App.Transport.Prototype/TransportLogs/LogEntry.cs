namespace Clarity.App.Transport.Prototype.TransportLogs
{
    public struct LogEntry
    {
        public LogEntryHeader Header;
        public string BodyStr;

        public override string ToString() => $"{Header} {BodyStr}";

        public static LogEntry Parse(string str)
        {
            return new LogEntry
            {
                Header = LogEntryHeader.Parse(str),
                BodyStr = str.Substring(39)
            };
        }
    }
}