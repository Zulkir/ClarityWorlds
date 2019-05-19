using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Clarity.Common.Infra.Files
{
    public static class FileSystemExtensions
    {
        public static void CopyFileTo(this IFileSystem sourceFileSystem, string sourcePath,
            IFileSystem destFileSystem, string destPath)
        {
            using (var destStream = destFileSystem.OpenWriteNew(destPath))
            using (var sourceStream = sourceFileSystem.OpenRead(sourcePath))
                sourceStream.CopyTo(destStream);
        }

        public static void CopyFolderTo(this IFileSystem sourceFileSystem, string sourceFolderPath,
            IFileSystem destFileSystem, string destFolderPath)
        {
            foreach (var file in sourceFileSystem.EnumerateFiles(sourceFolderPath))
                sourceFileSystem.CopyFileTo(file, destFileSystem, Path.Combine(destFolderPath, Path.GetFileName(file)));
            foreach (var folder in sourceFileSystem.EnumerateFolders(sourceFolderPath))
                sourceFileSystem.CopyFolderTo(folder, destFileSystem, Path.Combine(destFolderPath, Path.GetFileName(folder)));
        }

        public static IEnumerable<string> EnumerateFilesRecursively(this IFileSystem fileSystem, string folderPath)
        {
            return fileSystem.EnumerateFiles(folderPath)
                .Concat(fileSystem.EnumerateFolders(folderPath)
                .SelectMany(fileSystem.EnumerateFilesRecursively));
        }
    }
}