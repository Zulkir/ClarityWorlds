using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.Paths;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Mem
{
    public class TrwReaderMem : ITrwReader
    {
        private readonly IEnumerator<TrwMemToken> enumerator;
        private readonly TrwPathBuilder pathBuilder;

        private TrwMemToken currentToken;
        private int count;

        public TrwReaderMem(IEnumerable<TrwMemToken> tokens)
        {
            enumerator = tokens.GetEnumerator();
            pathBuilder = new TrwPathBuilder();
        }

        public TrwTokenType TokenType => currentToken.Type;
        public bool ValueAsBool => currentToken.ValueAsBool;
        public int ValueAsSInt32 => currentToken.ValueAsSInt32;
        public double ValueAsFloat64 => currentToken.ValueAsFloat64;
        public string ValueAsString => currentToken.ValueAsString;

        public int LineNumber => 1;
        public int LinePosition => count;
        public string CurrentEntryPath => pathBuilder.BuildPath();

        public void Dispose() => enumerator.Dispose();

        public bool MoveNext()
        {
            if (!enumerator.MoveNext())
                return false;
            currentToken = enumerator.Current;
            count++;
            pathBuilder.OnRead(this);
            return true;
        }

        public void Skip()
        {
            throw new System.NotImplementedException();
        }
    }
}