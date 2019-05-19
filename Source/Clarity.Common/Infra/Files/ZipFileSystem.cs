using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Clarity.Common.Infra.Files
{
    public class ZipFileSystem : IFileSystem
    {
        private readonly ZipArchive archive;

        public ZipFileSystem(ZipArchive archive)
        {
            this.archive = archive;
        }

        public bool FileExists(string path) => archive.GetEntry(path) != null;

        public bool FolderExists(string path)
        {
            var normalizedPath = new FilePath(path);
            return archive.Entries.Select(x => new FilePath(x.FullName)).Any(x => normalizedPath.IsSubpathOf(x));
        }

        public IEnumerable<string> EnumerateFiles(string folderPath)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> EnumerateFolders(string folderPath)
        {
            throw new System.NotImplementedException();
        }

        public byte[] ReadAllBytes(string path)
        {
            using (var memoryStream = new MemoryStream())
            using (var entryStream = OpenRead(path))
            {
                entryStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public Stream OpenRead(string path)
        {
            var entry = archive.GetEntry(path) ?? throw new Exception($"{path} not found.");
            return entry.Open();
        }

        public Stream OpenWriteNew(string path)
        {
            return archive.CreateEntry(path).Open();
        }

        public void DeleteFile(string path)
        {
            archive.GetEntry(path)?.Delete();
        }

        public void DeleteFolder(string path)
        {
            foreach (var folder in EnumerateFolders(path))
                DeleteFolder(folder);
            foreach (var file in EnumerateFiles(path))
                DeleteFile(file);
        }
    }
}