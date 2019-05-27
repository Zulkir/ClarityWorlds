using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Clarity.App.Transport.Prototype.SimLogs
{
    public class SimLogReader : IDisposable
    {
        private readonly GZipStream gzipStream;
        private readonly StreamReader reader;

        public SimLogReader(Stream rawStream, bool compressed)
        {
            if (compressed)
                gzipStream = new GZipStream(rawStream, CompressionMode.Decompress);
            reader = new StreamReader(compressed ? gzipStream : rawStream);
        }

        public void Dispose()
        {
            reader.Dispose();
            gzipStream?.Dispose();
        }

        public string ReadNext()
        {
            return reader.ReadLine();
        }

        public IEnumerable<SimLogEntry> ReadEntries()
        {
            while (!reader.EndOfStream)
                yield return SimLogEntry.Parse(reader.ReadLine());
        }
    }
}