using System.Text;
using Clarity.Common.Infra.TreeReadWrite;
using NUnit.Framework;

namespace Clarity.Core.Tests
{
    [TestFixture]
    public abstract class TrwWriterTestsBase
    {
        protected StringBuilder builder;
        protected ITrwWriter writer;

        [Test]
        public void Path()
        {
            Assert.That(writer.NextEntryPath, Is.EqualTo(""));
            writer.StartObject();
            Assert.That(writer.NextEntryPath, Is.EqualTo("???"));
            writer.AddProperty("a");
            Assert.That(writer.NextEntryPath, Is.EqualTo("a"));
            writer.WriteValue().Null();
            Assert.That(writer.NextEntryPath, Is.EqualTo("???"));
            writer.AddProperty("b");
            Assert.That(writer.NextEntryPath, Is.EqualTo("b"));
            writer.StartObject();
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.???"));
            writer.AddProperty("c");
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.c"));
            writer.StartArray(TrwValueType.Undefined);
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.c[0]"));
            writer.WriteValue().SInt32(123);
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.c[1]"));
            writer.StartObject();
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.c[1].???"));
            writer.AddProperty("d");
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.c[1].d"));
            writer.StartArray(TrwValueType.Float64);
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.c[1].d[0]"));
            writer.EndArray();
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.c[1].???"));
            writer.EndObject();
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.c[2]"));
            writer.EndArray();
            Assert.That(writer.NextEntryPath, Is.EqualTo("b.???"));
            writer.EndObject();
            Assert.That(writer.NextEntryPath, Is.EqualTo("???"));
            writer.EndObject();
            Assert.That(writer.NextEntryPath, Is.EqualTo(""));
        }
    }
}