using System.IO;
using System.Text;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Formats.Xml;
using NUnit.Framework;

namespace Clarity.Core.Tests
{
    [TestFixture]
    public class TrwWriterXmlTests : TrwWriterTestsBase
    {
        private const string XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n";

        private string GetText()
        {
            writer.Dispose();
            return builder.ToString();
        }

        [SetUp]
        public void Setup()
        {
            builder = new StringBuilder();
            writer = new TrwWriterXml(builder);
        }

        [Test]
        public void CloseStream()
        {
            var stream = new MemoryStream();
            writer = new TrwWriterXml(stream);
            writer.Dispose();
            Assert.That(() => stream.Position = 0, Throws.Exception);
        }

        [Test]
        public void ThrowOnValuelessProperty()
        {
            writer.StartObject();
            writer.AddProperty("a");
            Assert.That(() => writer.EndObject(), Throws.Exception);
        }

        [Test]
        public void SimpleProperties()
        {
            writer.StartObject();
            writer.AddProperty("a");
            writer.WriteValue().SInt32(123);
            writer.AddProperty("b");
            writer.WriteValue().Float64(345.678);
            writer.EndObject();
            var text = GetText();
            Assert.That(text, Is.EqualTo(XmlHeader + "<Root>\r\n  <a>123</a>\r\n  <b>345.678</b>\r\n</Root>"));
        }

        [Test]
        public void RefProperties()
        {
            writer.StartObject();
            writer.AddProperty("a");
            writer.WriteValue().Null();
            writer.AddProperty("b");
            writer.WriteValue().String("qwe");
            writer.EndObject();
            var text = GetText();
            Assert.That(text, Is.EqualTo(XmlHeader + "<Root>\r\n  <a null=\"True\" />\r\n  <b>\"qwe\"</b>\r\n</Root>"));
        }

        [Test]
        public void Objects()
        {
            writer.StartObject();
            writer.AddProperty("a");
            writer.StartObject();
            writer.AddProperty("b");
            writer.WriteValue().SInt32(123);
            writer.AddProperty("c");
            writer.StartObject();
            writer.AddProperty("d");
            writer.WriteValue().String("qwe");
            writer.EndObject();
            writer.AddProperty("e");
            writer.WriteValue().Float64(123.456);
            writer.EndObject();
            writer.EndObject();
            var text = GetText();
            Assert.That(text, Is.EqualTo(XmlHeader +
@"<Root>
  <a>
    <b>123</b>
    <c>
      <d>""qwe""</d>
    </c>
    <e>123.456</e>
  </a>
</Root>"));
        }

        [Test]
        public void ValueArrays()
        {
            writer.StartObject();
            writer.AddProperty("a");
            writer.StartArray(TrwValueType.Float32);
            writer.WriteValue().Float32(123);
            writer.WriteValue().Float32(234.567f);
            writer.WriteValue().Float32(0);
            writer.EndArray();
            writer.EndObject();
            var text = GetText();
            Assert.That(text, Is.EqualTo(XmlHeader +
@"<Root>
  <a>[123, 234.567, 0]</a>
</Root>"));
        }

        [Test]
        public void UnstructuredArrays()
        {
            writer.StartObject();
            writer.AddProperty("a");
            writer.StartArray(TrwValueType.Undefined);
            writer.WriteValue().Float32(123);
            writer.WriteValue().Null();
            writer.WriteValue().String("qwe");
            writer.StartObject();
            writer.AddProperty("b");
            writer.WriteValue().SInt32(234);
            writer.EndObject();
            writer.StartArray(TrwValueType.Int32);
            writer.WriteValue().SInt32(34);
            writer.WriteValue().SInt32(56);
            writer.EndArray();
            writer.StartArray(TrwValueType.Undefined);
            writer.WriteValue().SInt32(987);
            writer.EndArray();
            writer.EndArray();
            writer.EndObject();
            var text = GetText();
            Assert.That(text, Is.EqualTo(XmlHeader +
@"<Root>
  <a array=""True"">
    <Elem>123</Elem>
    <Elem null=""True"" />
    <Elem>""qwe""</Elem>
    <Elem>
      <b>234</b>
    </Elem>
    <Elem>[34, 56]</Elem>
    <Elem array=""True"">
      <Elem>987</Elem>
    </Elem>
  </a>
</Root>"));
        }

        [Test]
        public void NestedArraysOfDifferentTypes()
        {
            writer.StartObject();
            writer.AddProperty("a");
            writer.StartArray(TrwValueType.Undefined);
            writer.StartArray(TrwValueType.Int32);
            writer.WriteValue().SInt32(123);
            writer.WriteValue().SInt32(234);
            writer.EndArray();
            writer.WriteValue().SInt32(345);
            writer.EndArray();
            writer.EndObject();
            var text = GetText();
            Assert.That(text, Is.EqualTo(XmlHeader +
@"<Root>
  <a array=""True"">
    <Elem>[123, 234]</Elem>
    <Elem>345</Elem>
  </a>
</Root>"));
        }

        [Test]
        public void PropertyAfterArray()
        {
            writer.StartObject();
            writer.AddProperty("a");
            writer.StartArray(TrwValueType.Undefined);
            writer.EndArray();
            writer.AddProperty("b");
            writer.StartObject();
            writer.EndObject();
            writer.EndObject();
            var text = GetText();
            Assert.That(text, Is.EqualTo(XmlHeader +
@"<Root>
  <a array=""True"" />
  <b />
</Root>"));
        }
    }
}