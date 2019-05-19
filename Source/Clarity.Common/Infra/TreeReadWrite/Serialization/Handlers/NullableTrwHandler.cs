using System.Diagnostics;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class NullableTrwHandler<T> : TrwSerializationHandlerBase<T?> where T : struct
    {
        public override bool ContentIsProperties { get; }

        public NullableTrwHandler(bool contentIsProps)
        {
            ContentIsProperties = contentIsProps;
        }

        public override void SaveContent(ITrwSerializationWriteContext context, T? value)
        {
            Debug.Assert(value != null, nameof(value) + " != null");
            context.Write(value.Value);
        }

        public override T? LoadContent(ITrwSerializationReadContext context)
        {
            return context.Read<T>();
        }
    }
}