using System.IO;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Formats.Json;
using NUnit.Framework;

namespace Clarity.Core.Tests
{
    [TestFixture]
    public class TrwReaderJsonTests
    {
        private ITrwReader reader;
        private MemoryStream stream;
        private StreamWriter streamWriter;

        private void SetText(string text)
        {
            streamWriter.Write(text);
            streamWriter.Flush();
            stream.Position = 0;
            reader = new TrwReaderJson(stream);
        }

        [SetUp]
        public void Setup()
        {
            stream = new MemoryStream();
            streamWriter = new StreamWriter(stream);
        }

        [TearDown]
        public void Cleanup()
        {
            streamWriter.Dispose();
        }

        private void ReadTrue()
        {
            Assert.That(reader.MoveNext(), Is.True);
        }

        [Test]
        public void Path()
        {
            SetText(@"
{
    ""a"": null,
    ""b"": {
        ""c"": [
            123,
            {
                ""d"": []
            }
        ]
    }
}");

            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo(""));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c[0]"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c[1]"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c[1].d"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c[1].d"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c[1].d"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c[1]"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b.c"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo(""));
            Assert.That(reader.MoveNext(), Is.False);
        }

        [Test]
        public void PathWhenSkipping()
        {
            SetText(
@"{
    ""a"": [
        123, 
        ""qwe"", 
        ""qwe"", 
        {
            ""c"": 234
        }
    ],
    ""b"": [
        {""a"": 123, ""b"":{""c"":234.0}},
        345
    ]
}");

            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo(""));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("a"));
            reader.Skip();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b[0]"));
            reader.Skip();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b[1]"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.CurrentEntryPath, Is.EqualTo(""));
            Assert.That(reader.MoveNext(), Is.False);
        }
    }
}