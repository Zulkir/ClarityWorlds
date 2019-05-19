using System.Collections.Generic;
using System.IO;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public interface ITrwFormat
    {
        string Name { get; }
        IReadOnlyList<string> FileExtensions { get; }
        ITrwWriter CreateWriter(Stream stream);
        ITrwReader CreateReader(Stream stream);
    }
}