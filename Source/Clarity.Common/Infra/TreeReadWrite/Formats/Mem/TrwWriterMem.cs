using System;
using Clarity.Common.Infra.TreeReadWrite.Paths;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Mem
{
    public class TrwWriterMem : ITrwWriter, ITrwValueWriter
    {
        private readonly Action<TrwMemToken> consumer;
        private readonly Action onDispose;
        private readonly TrwPathBuilder pathBuilder;

        public TrwWriterMem(Action<TrwMemToken> consumer, Action onDispose)
        {
            this.consumer = consumer;
            this.onDispose = onDispose;
            pathBuilder = new TrwPathBuilder();
        }

        public void Dispose()
        {
            onDispose();
        }

        public void AddProperty(string name)
        {
            consumer(TrwMemToken.PropertyName(name));
            pathBuilder.OnProperty(name);
        }

        public void StartObject()
        {
            consumer(TrwMemToken.StartObject());
            pathBuilder.OnStartObject();
        }

        public void EndObject()
        {
            consumer(TrwMemToken.EndObject());
            pathBuilder.OnEndObject();
        }

        public void StartArray(TrwValueType arrayType)
        {
            consumer(TrwMemToken.StartArray());
            pathBuilder.OnStartArray();
        }

        public void EndArray()
        {
            consumer(TrwMemToken.EndArray());
            pathBuilder.OnEndArray();
        }

        public ITrwValueWriter WriteValue() => this;

        public void Flush() {}

        public string NextEntryPath => pathBuilder.BuildPath();

        public void Null()
        {
            consumer(TrwMemToken.Null());
            pathBuilder.OnValue();
        }

        public void String(string val)
        {
            consumer(TrwMemToken.String(val));
            pathBuilder.OnValue();
        }
        
        public void SInt32(int val)
        {
            consumer(TrwMemToken.Integer(val));
            pathBuilder.OnValue();
        }
        
        public void Float32(float val)
        {
            consumer(TrwMemToken.Float(val));
            pathBuilder.OnValue();
        }

        public void Float64(double val)
        {
            consumer(TrwMemToken.Float(val));
            pathBuilder.OnValue();
        }

        public void Bool(bool val)
        {
            consumer(TrwMemToken.Boolean(val));
            pathBuilder.OnValue();
        }
    }
}