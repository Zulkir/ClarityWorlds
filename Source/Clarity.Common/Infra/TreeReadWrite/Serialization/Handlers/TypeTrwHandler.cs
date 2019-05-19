using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class TypeTrwHandler : TrwSerializationHandlerBase<Type>
    {
        public override bool ContentIsProperties => false;
        public override void SaveContent(ITrwSerializationWriteContext context, Type value) => context.WriteType(value);
        public override Type LoadContent(ITrwSerializationReadContext context) => context.ReadType();
    }
}