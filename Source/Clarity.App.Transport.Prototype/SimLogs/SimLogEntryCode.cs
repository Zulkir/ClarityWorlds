namespace Clarity.App.Transport.Prototype.SimLogs
{
    public enum SimLogEntryCode : short
    {
        Read = 101,
        Update = 301,
        Create = 305,
        MigrationStart = 701,
        NewCopy = 702,
        RemoveCopy = 703,
        MigrationEnd = 799
    }
}