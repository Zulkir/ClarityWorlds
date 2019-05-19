using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class GuidTrwHandler : TrwSerializationHandlerBase<Guid>
    {
        public override bool ContentIsProperties => false;

        public override void SaveContent(ITrwSerializationWriteContext context, Guid value)
        {
            context.Writer.WriteValue().String(value.ToString().ToUpper());
        }

        public override Guid LoadContent(ITrwSerializationReadContext context)
        {
            return Guid.Parse(context.Read<string>());
        }
    }
}