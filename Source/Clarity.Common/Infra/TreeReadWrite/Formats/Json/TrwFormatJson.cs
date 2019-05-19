using System.Collections.Generic;
using System.IO;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Json
{
    public class TrwFormatJson : ITrwFormat
    {
        public IReadOnlyList<string> FileExtensions { get; } = new[] { ".json" };
        public string Name => "json";

        public ITrwWriter CreateWriter(Stream stream) =>
            new TrwWriterJson(stream);

        public ITrwReader CreateReader(Stream stream) => 
            new TrwReaderJson(stream);
    }
}