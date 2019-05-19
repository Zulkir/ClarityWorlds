using System.Collections.Generic;
using System.IO;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Xml
{
    public class TrwFormatXml : ITrwFormat
    {
        public IReadOnlyList<string> FileExtensions { get; } = new[] {".xml"};
        public string Name => "xml";

        public ITrwWriter CreateWriter(Stream stream) => 
            new TrwWriterXml(stream);

        public ITrwReader CreateReader(Stream stream) => 
            new TrwReaderXml(stream);
    }
}