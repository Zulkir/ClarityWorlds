namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class BoolTrwHandler : TrwSerializationHandlerBase<bool>
    {
        public override bool ContentIsProperties => false;
        public override void SaveContent(ITrwSerializationWriteContext context, bool value) => context.Writer.WriteValue().Bool(value);
        public override bool LoadContent(ITrwSerializationReadContext context) => context.ReadFluent(context.Reader.ValueAsBool);
    }
}