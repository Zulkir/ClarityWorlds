namespace Clarity.App.Transport.Prototype.TransportLogs
{
    public struct LogEntryHeader
    {
        public int Sequence;
        public long Systime;
        public LogEntryCode Code;
        public int AppId;

        public override string ToString() => 
            $"{Sequence,10} {Systime,16} {(int)Code,4} {AppId,5}";

        public static LogEntryHeader Parse(string str)
        {
            return new LogEntryHeader
            {
                Sequence = int.Parse(str.Substring(0, 10)),
                Systime = long.Parse(str.Substring(11, 16)),
                Code = (LogEntryCode)int.Parse(str.Substring(28, 4)),
                AppId = int.Parse(str.Substring(33, 5))
            };
        }
    }
}