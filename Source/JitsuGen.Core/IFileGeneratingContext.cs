using System;

namespace JitsuGen.Core
{
    public interface IFileGeneratingContext : IDisposable
    {
        string FileType { get; }
        void AddFile(GeneratedFileInfo fileInfo);
    }
}