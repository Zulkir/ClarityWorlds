using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Clarity.Common.Infra.Files
{
    public class MemoryFileSystem : IFileSystem
    {
        private readonly Dictionary<string, byte[]> files;

        public MemoryFileSystem()
        {
            files = new Dictionary<string, byte[]>();
        }

        private static string Normalize(string path)
        {
            if (path == null)
                return null;
            if (path.Contains("\\"))
                path = path.Replace('\\', '/');
            while (path.StartsWith("/"))
                path = path.Substring(1);
            while (path.EndsWith("/"))
                path = path.Substring(0, path.Length - 1);
            if (path.Contains(".."))
                throw new NotImplementedException();
            return path;
        }

        public bool FileExists(string path) => files.ContainsKey(Normalize(path));

        public bool FolderExists(string path)
        {
            var normalizedPath = Normalize(path);
            return files.Keys.Any(x => x.StartsWith(normalizedPath));
        }

        private IEnumerable<string> EnumerateAllFolders()
        {
            var hashSet = new HashSet<string>();
            foreach (var file in files.Keys)
            {
                var rawFolderPath = Path.GetDirectoryName(file);
                while (!string.IsNullOrEmpty(rawFolderPath))
                {
                    hashSet.Add(Normalize(rawFolderPath));
                    rawFolderPath = Path.GetDirectoryName(rawFolderPath);
                }
            }
            return hashSet;
        }

        public IEnumerable<string> EnumerateFiles(string folderPath)
        {
            var normalizedFolderPath = Normalize(folderPath);
            return files.Keys.Where(x => Normalize(Path.GetDirectoryName(x)) == normalizedFolderPath);
        }

        public IEnumerable<string> EnumerateFolders(string folderPath)
        {
            var normalizedFolderPath = Normalize(folderPath);
            return EnumerateAllFolders().Where(x => Normalize(x) != "" && Normalize(Path.GetDirectoryName(x)) == normalizedFolderPath);
        }

        public byte[] ReadAllBytes(string path)
        {
            return files[Normalize(path)];
        }

        public Stream OpenRead(string path)
        {
            if (!files.TryGetValue(Normalize(path), out var bytes))
                throw new FileNotFoundException();
            return new MemoryStream(bytes, false);
        }

        public Stream OpenWriteNew(string path)
        {
            return new MemoryFileSystemWriteStream(x => files[Normalize(path)] = x);
        }

        public void DeleteFile(string path)
        {
            files.Remove(Normalize(path));
        }

        public void DeleteFolder(string path)
        {
            var normalizedPath = Normalize(path);
            var deathNote = files.Keys.Where(x => x.StartsWith(normalizedPath));
            foreach (var key in deathNote)
                files.Remove(key);
        }
    }
}