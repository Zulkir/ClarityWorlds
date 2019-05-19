using System;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public interface ITrwReader : IDisposable
    {
        bool MoveNext();
        void Skip();
        TrwTokenType TokenType { get; }
        bool ValueAsBool { get; }
        int ValueAsSInt32 { get; }
        double ValueAsFloat64 { get; }
        string ValueAsString { get; }

        int LineNumber { get; }
        int LinePosition { get; }
        string CurrentEntryPath { get; }
    }
}