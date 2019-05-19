namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class StringTrwHandler : TrwSerializationHandlerBase<string>
    {
        public override bool ContentIsProperties => false;
        public override void SaveContent(ITrwSerializationWriteContext context, string value) => context.Writer.WriteValue().String(value);
        public override string LoadContent(ITrwSerializationReadContext context) => context.ReadFluent(context.Reader.ValueAsString);
    }
}