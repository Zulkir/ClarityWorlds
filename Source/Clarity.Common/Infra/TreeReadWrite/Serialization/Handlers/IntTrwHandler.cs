namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class IntTrwHandler : TrwSerializationHandlerBase<int>
    {
        public override bool ContentIsProperties => false;
        public override void SaveContent(ITrwSerializationWriteContext context, int value) => context.Writer.WriteValue().SInt32(value);
        public override int LoadContent(ITrwSerializationReadContext context) => context.ReadFluent(context.Reader.ValueAsSInt32);
    }
}