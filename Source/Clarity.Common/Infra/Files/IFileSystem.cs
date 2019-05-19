using System.IO;

namespace Clarity.Common.Infra.Files
{
    public interface IFileSystem : IReadOnlyFileSystem
    {
        Stream OpenWriteNew(string path);
        void DeleteFile(string path);
        void DeleteFolder(string path);
    }
}