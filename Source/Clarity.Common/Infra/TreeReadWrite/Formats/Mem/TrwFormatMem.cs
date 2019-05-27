using System;
using System.Collections.Generic;
using System.IO;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Mem
{
    public class TrwFormatMem : ITrwFormat
    {
        public string Name => "mem";
        
        public IReadOnlyList<string> FileExtensions { get; } = new[] { ".mem" };

        public ITrwWriter CreateWriter(Stream stream)
        {
            throw new InvalidOperationException("mem format does not support Stream read.write");
        }

        public ITrwReader CreateReader(Stream stream)
        {
            throw new InvalidOperationException("mem format does not support Stream read.write");
        }
    }
}