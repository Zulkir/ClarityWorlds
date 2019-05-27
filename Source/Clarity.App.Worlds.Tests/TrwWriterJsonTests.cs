using System.IO;
using System.Text;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Formats.Json;
using NUnit.Framework;

namespace Clarity.App.Worlds.Tests
{
    [TestFixture]
    public class TrwWriterJsonTests : TrwWriterTestsBase
    {
        private string GetText()
        {
            writer.Dispose();
            return builder.ToString();
        }

        [SetUp]
        public void Setup()
        {
            builder = new StringBuilder();
            writer = new TrwWriterJson(builder);
        }

        [Test]
        public void CloseStream()
        {
            var stream = new MemoryStream();
            writer = new TrwWriterJson(stream);
            writer.Dispose();
            Assert.That(() => stream.Position = 0, Throws.Exception);
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
            Assert.That(text, Is.EqualTo("{\r\n  \"a\": 123,\r\n  \"b\": 345.678\r\n}"));

        }


        [Test]
        public void NestedObjects()
        {
            writer.StartObject();
                writer.AddProperty("a");
                writer.WriteValue().String("A");
                writer.AddProperty("object");
                writer.StartObject();
                    writer.AddProperty("b");
                    writer.WriteValue().String("B");
                writer.EndObject();
            writer.EndObject();

            var text = GetText();
            Assert.That(text, Is.EqualTo(@"{
  ""a"": ""A"",
  ""object"": {
    ""b"": ""B""
  }
}"));
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
            Assert.That(text, Is.EqualTo("{\r\n  \"a\": null,\r\n  \"b\": \"qwe\"\r\n}"));
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
            Assert.That(text, Is.EqualTo(@"{
  ""a"": {
    ""b"": 123,
    ""c"": {
      ""d"": ""qwe""
    },
    ""e"": 123.456
  }
}"));
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
            Assert.That(text, Is.EqualTo(@"{
  ""a"": [
    123.0,
    234.567,
    0.0
  ]
}"));
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

            Assert.That(text, Is.EqualTo(@"{
  ""a"": [
    123.0,
    null,
    ""qwe"",
    {
      ""b"": 234
    },
    [
      34,
      56
    ],
    [
      987
    ]
  ]
}"));
        }
    }
}