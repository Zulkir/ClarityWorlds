using System.IO;
using System.Text;

namespace JitsuGen.Core
{
    public class DirectFileWriter : IFileWriter
    {
        public bool TryReadFile(string path, out string content)
        {
            if (!File.Exists(path))
            {
                content = null;
                return false;
            }
            content = File.ReadAllText(path);
            return true;
        }

        public void WriteFile(string path, string content, Encoding encoding)
        {
            EnsureDirectoryExists(path);
            File.WriteAllText(path, content, encoding);
        }

        private void EnsureDirectoryExists(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }
}