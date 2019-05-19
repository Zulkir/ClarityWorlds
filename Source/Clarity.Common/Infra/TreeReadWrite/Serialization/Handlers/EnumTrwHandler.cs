using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class EnumTrwHandler<T> : TrwSerializationHandlerBase<T>
    {
        public override bool ContentIsProperties => false;
        public override void SaveContent(ITrwSerializationWriteContext context, T value) => context.Writer.WriteValue().String(value.ToString());
        public override T LoadContent(ITrwSerializationReadContext context) => context.ReadFluent((T)Enum.Parse(typeof(T), context.Reader.ValueAsString));
    }
}