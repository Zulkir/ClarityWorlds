using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Json
{
    public class TrwWriterJson : ITrwWriter, ITrwValueWriter
    {
        private readonly JsonWriter writer;
        private readonly TrwPathBuilder pathBuilder = new TrwPathBuilder();

        public TrwWriterJson(Stream stream)
        {
            writer = new JsonTextWriter(new StreamWriter(stream, Encoding.UTF8))
            {
                Formatting = Formatting.Indented,
                CloseOutput = true
            };
        }

        public TrwWriterJson(StringBuilder stringBuilder)
        {
            writer = new JsonTextWriter(new StringWriter(stringBuilder))
            {
                Formatting = Formatting.Indented
            };
        }

        public void Dispose()
        {
            writer.Close();
        }

        public void AddProperty(string name)
        {
            writer.WritePropertyName(name);
            pathBuilder.OnProperty(name);
        }

        public void StartObject()
        {
            writer.WriteStartObject();
            pathBuilder.OnStartObject();
        }

        public void EndObject()
        {
            writer.WriteEndObject();
            pathBuilder.OnEndObject();
        }

        public void StartArray(TrwValueType arrayType)
        {
            writer.WriteStartArray();
            pathBuilder.OnStartArray();
        }

        public void EndArray()
        {
            writer.WriteEndArray();
            pathBuilder.OnEndArray();
        }

        public ITrwValueWriter WriteValue() => this;

        public void Flush() => writer.Flush();

        public string NextEntryPath => pathBuilder.BuildPath();

        #region Values
        public void Null() => WriteValue<object>(null, (w, v) => w.WriteNull());
        public void String(string val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void Bool(bool val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void SInt8(sbyte val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void UInt8(byte val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void SInt16(short val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void UInt16(ushort val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void SInt32(int val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void UInt32(uint val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void SInt64(long val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void UInt64(ulong val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void Float32(float val) => WriteValue(val, (w, v) => w.WriteValue(v));
        public void Float64(double val) => WriteValue(val, (w, v) => w.WriteValue(v));
        #endregion

        private void WriteValue<T>(T value, Action<JsonWriter, T> write)
        {
            write(writer, value);
            pathBuilder.OnValue();
        }
    }
}