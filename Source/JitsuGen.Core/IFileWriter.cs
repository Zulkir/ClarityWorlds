using System.Text;

namespace JitsuGen.Core
{
    public interface IFileWriter
    {
        bool TryReadFile(string path, out string content);
        void WriteFile(string path, string content, Encoding encoding);
    }
}