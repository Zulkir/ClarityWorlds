using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class Vector4TrwHandler : TrwSerializationHandlerBase<Vector4>
    {
        public override bool ContentIsProperties => false;

        public override void SaveContent(ITrwSerializationWriteContext context, Vector4 value)
        {
            context.Writer.StartArray(TrwValueType.Float32);
            context.Writer.WriteValue().Float32(value.X);
            context.Writer.WriteValue().Float32(value.Y);
            context.Writer.WriteValue().Float32(value.Z);
            context.Writer.WriteValue().Float32(value.W);
            context.Writer.EndArray();
        }

        public override Vector4 LoadContent(ITrwSerializationReadContext context)
        {
            context.Reader.CheckAndMoveNext(TrwTokenType.StartArray);
            var x = context.Read<float>();
            var y = context.Read<float>();
            var z = context.Read<float>();
            var w = context.Read<float>();
            context.Reader.CheckAndMoveNext(TrwTokenType.EndArray);
            return new Vector4(x, y, z, w);
        }
    }
}