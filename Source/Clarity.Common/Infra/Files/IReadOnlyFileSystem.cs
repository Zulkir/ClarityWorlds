using System.Collections.Generic;
using System.IO;

namespace Clarity.Common.Infra.Files 
{
    public interface IReadOnlyFileSystem
    {
        bool FileExists(string path);
        bool FolderExists(string path);
        IEnumerable<string> EnumerateFiles(string folderPath);
        IEnumerable<string> EnumerateFolders(string folderPath);
        byte[] ReadAllBytes(string path);
        Stream OpenRead(string path);
    }
}