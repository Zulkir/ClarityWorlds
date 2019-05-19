using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class Vector2TrwHandler : TrwSerializationHandlerBase<Vector2>
    {
        public override bool ContentIsProperties => false;

        public override void SaveContent(ITrwSerializationWriteContext context, Vector2 value)
        {
            context.Writer.StartArray(TrwValueType.Float32);
            context.Writer.WriteValue().Float32(value.X);
            context.Writer.WriteValue().Float32(value.Y);
            context.Writer.EndArray();
        }

        public override Vector2 LoadContent(ITrwSerializationReadContext context)
        {
            context.Reader.CheckAndMoveNext(TrwTokenType.StartArray);
            var x = context.Read<float>();
            var y = context.Read<float>();
            context.Reader.CheckAndMoveNext(TrwTokenType.EndArray);
            return new Vector2(x, y);
        }
    }
}