using System.IO;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public static class TrwFactoryExtensions
    {
        public static ITrwReader JsonReader(this ITrwFactory factory, Stream stream) =>
            factory.Reader(stream, "json");

        public static ITrwWriter JsonWriter(this ITrwFactory factory, Stream stream) =>
            factory.Writer(stream, "json");

        public static ITrwReader XmlReader(this ITrwFactory factory, Stream stream) =>
            factory.Reader(stream, "xml");

        public static ITrwWriter XmlWriter(this ITrwFactory factory, Stream stream) =>
            factory.Writer(stream, "xml");

        public static ITrwReader MemReader(this ITrwFactory factory, Stream stream) =>
            factory.Reader(stream, "mem");

        public static ITrwWriter MemWriter(this ITrwFactory factory, Stream stream) =>
            factory.Writer(stream, "mem");
    }
}