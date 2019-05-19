namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class FloatTrwHandler : TrwSerializationHandlerBase<float>
    {
        public override bool ContentIsProperties => false;
        public override void SaveContent(ITrwSerializationWriteContext context, float value) => context.Writer.WriteValue().Float32(value);
        public override float LoadContent(ITrwSerializationReadContext context) => context.ReadFluent((float)context.Reader.ValueAsFloat64);
    }
}