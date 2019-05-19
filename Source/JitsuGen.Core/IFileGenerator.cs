using System;

namespace JitsuGen.Core
{
    public interface IFileGenerator
    {
        string FileType { get; }
        void GenerateFor(IFileGeneratingContext context, Type type);
    }
}