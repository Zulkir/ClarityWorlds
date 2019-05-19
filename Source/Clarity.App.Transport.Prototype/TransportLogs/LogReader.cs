using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Clarity.App.Transport.Prototype.TransportLogs
{
    public class LogReader : IDisposable
    {
        private readonly GZipStream gzipStream;
        private readonly StreamReader reader;

        public LogReader(Stream rawStream)
        {
            gzipStream = new GZipStream(rawStream, CompressionMode.Decompress);
            reader = new StreamReader(gzipStream);
        }

        public void Dispose()
        {
            reader.Dispose();
            gzipStream.Dispose();
        }

        public string ReadNext()
        {
            return reader.ReadLine();
        }

        public IEnumerable<LogEntry> ReadEntries()
        {
            while (!reader.EndOfStream)
                yield return LogEntry.Parse(reader.ReadLine());
        }
    }
}