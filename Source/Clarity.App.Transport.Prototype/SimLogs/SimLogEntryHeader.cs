namespace Clarity.App.Transport.Prototype.SimLogs
{
    public struct SimLogEntryHeader
    {
        public int Sequence;
        public long Systime;
        public SimLogEntryCode Code;
        public int AppId;

        public override string ToString() => 
            $"{Sequence,10} {Systime,16} {(int)Code,4} {AppId,5}";

        public static SimLogEntryHeader Parse(string str)
        {
            return new SimLogEntryHeader
            {
                Sequence = int.Parse(str.Substring(0, 10)),
                Systime = long.Parse(str.Substring(11, 16)),
                Code = (SimLogEntryCode)int.Parse(str.Substring(28, 4)),
                AppId = int.Parse(str.Substring(33, 5))
            };
        }
    }
}