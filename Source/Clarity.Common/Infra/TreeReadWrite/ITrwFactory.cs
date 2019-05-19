using System.IO;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public interface ITrwFactory
    {
        ITrwReader Reader(Stream stream, string format);
        ITrwWriter Writer(Stream stream, string format);
    }
}