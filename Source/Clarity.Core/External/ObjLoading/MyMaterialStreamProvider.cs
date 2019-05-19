using System.IO;
using Clarity.Common.Infra.Files;
using ObjLoader.Loader.Loaders;

namespace Clarity.Core.External.ObjLoading
{
    public class MyMaterialStreamProvider : IMaterialStreamProvider
    {
        private readonly string relativePath;
        private readonly IFileSystem fileSystem;

        public MyMaterialStreamProvider(string relativePath, IFileSystem fileSystem)
        {
            this.relativePath = relativePath;
            this.fileSystem = fileSystem;
        }

        public Stream Open(string materialFilePath)
        {
            return fileSystem.OpenRead(Path.Combine(relativePath, materialFilePath));
        }
    }
}