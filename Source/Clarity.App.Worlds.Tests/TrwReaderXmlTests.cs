using System.IO;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Formats.Xml;
using NUnit.Framework;

namespace Clarity.App.Worlds.Tests
{
    [TestFixture]
    public class TrwReaderXmlTests
    {
        private ITrwReader reader;
        private MemoryStream stream;
        private StreamWriter streamWriter;

        private void SetText(string text)
        {
            streamWriter.Write(text);
            streamWriter.Flush();
            stream.Position = 0;
            reader = new TrwReaderXml(stream);
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
        public void ThrowOnNoRoot()
        {
            SetText(@"<Blabla><a>123</a></Blabla>");
            Assert.That(() => reader.MoveNext(), Throws.Exception);
        }

        [Test]
        public void SimpleProperties()
        {
            SetText(@"<Root><a>123</a><b>345.678</b><c>true</c></Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(123));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Float));
            Assert.That(reader.ValueAsFloat64, Is.EqualTo(345.678));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("c"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Boolean));
            Assert.That(reader.ValueAsBool, Is.EqualTo(true));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }

        [Test]
        public void RefProperties()
        {
            SetText(@"<Root><a null=""True""/><b>""qwe""</b></Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Null));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.String));
            Assert.That(reader.ValueAsString, Is.EqualTo("qwe"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }

        [Test]
        public void Objects()
        {
            SetText(@"<Root>
                        <a>
                            <b>
                                <c>123</c>
                            </b>
                            <d>
                                <e>234.567</e>
                            </d>
                            <f>456</f>
                        </a>
                    </Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("c"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("d"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("e"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Float));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("f"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }

        [Test]
        public void ValueArrays()
        {
            SetText(@"<Root><a>[123, 234.567, 0]</a></Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(123));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Float));
            Assert.That(reader.ValueAsFloat64, Is.EqualTo(234.567));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(0));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }

        [Test]
        public void UnstructuredArrays()
        {
            SetText(
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
</Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(123));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Null));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.String));
            Assert.That(reader.ValueAsString, Is.EqualTo("qwe"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(234));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(34));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(56));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(987));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }

        // todo: more skip cases
        [Test]
        public void Skip()
        {
            SetText(
@"<Root>
    <a array=""True"">
        <Elem>123</Elem>
        <Elem>""qwe""</Elem>
        <Elem>""qwe""</Elem>
        <Elem>
            <c>234</c>
        </Elem>
    </a>
    <b>123</b>
</Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            reader.Skip();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            Assert.That(reader.ValueAsSInt32, Is.EqualTo(123));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }

        [Test]
        public void EmptyObjects()
        {
            SetText(@"<Root><a></a><b /><c>123</c></Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            ReadTrue();
            Assert.That(reader.ValueAsString, Is.EqualTo("c"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }

        [Test]
        public void EmptyArray()
        {
            SetText(@"<Root><a array=""True""/><b>123</b></Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndArray));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.Integer));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }

        [Test]
        public void EmptyObjectAfterRoot()
        {
            SetText(@"<Root><a><b /></a></Root>");
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("a"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.PropertyName));
            Assert.That(reader.ValueAsString, Is.EqualTo("b"));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.StartObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            ReadTrue();
            Assert.That(reader.TokenType, Is.EqualTo(TrwTokenType.EndObject));
            Assert.That(reader.MoveNext(), Is.False);
        }
        
        [Test]
        public void Path()
        {
            SetText(@"
<Root>
    <a null=""True"" />
    <b>
        <c array=""True"">
            <Elem>123</Elem>
            <Elem>
                <d array=""True""></d>
            </Elem>
        </c>
    </b>
</Root>");

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
@"<Root>
    <a array=""True"">
        <Elem>123</Elem>
        <Elem>""qwe""</Elem>
        <Elem>""qwe""</Elem>
        <Elem>
            <c>234</c>
        </Elem>
    </a>
    <b array=""True"">
        <Elem><a>123</a><b><c>234.0</c></b></Elem>
        <Elem>345</Elem>
    </b>
</Root>");

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