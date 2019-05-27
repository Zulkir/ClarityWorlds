using System.Collections.Generic;
using System.IO;

namespace Clarity.Common.Infra.Files
{
    public class ActualFileSystem : IFileSystem
    {
        private readonly string basePath;

        public ActualFileSystem(string basePath = null)
        {
            this.basePath = string.IsNullOrEmpty(basePath) ? null : basePath;
        }

        private string AdjustPath(string path) =>
            basePath != null ? Path.Combine(basePath, path) : path;

        public bool FileExists(string path) => File.Exists(AdjustPath(path));
        public bool FolderExists(string path) => Directory.Exists(AdjustPath(path));
        public IEnumerable<string> EnumerateFiles(string folderPath) => Directory.EnumerateFiles(AdjustPath(folderPath));
        public IEnumerable<string> EnumerateFolders(string folderPath) => Directory.EnumerateDirectories(AdjustPath(folderPath));
        public byte[] ReadAllBytes(string path) => File.ReadAllBytes(AdjustPath(path));
        public Stream OpenRead(string path) => File.OpenRead(AdjustPath(path));
        public Stream OpenWriteNew(string path) => File.Create(AdjustPath(path));
        public void DeleteFile(string path) => File.Delete(AdjustPath(path));
        public void DeleteFolder(string path) => Directory.Delete(AdjustPath(path), true);

        public static ActualFileSystem Singleton { get; } = new ActualFileSystem();
    }
}