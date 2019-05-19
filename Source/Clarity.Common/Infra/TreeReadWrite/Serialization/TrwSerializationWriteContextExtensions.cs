using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public static class TrwSerializationWriteContextExtensions
    {
        public static void WriteProperty<T>(this ITrwSerializationWriteContext context, string name, T value)
        {
            context.Writer.AddProperty(name);
            context.Write(value);
        }

        public static void WriteProperty(this ITrwSerializationWriteContext context, string name, Type type, object value)
        {
            context.Writer.AddProperty(name);
            context.Write(type, value);
        }
    }
}