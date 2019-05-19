using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class Vector3TrwHandler : TrwSerializationHandlerBase<Vector3>
    {
        public override bool ContentIsProperties => false;

        public override void SaveContent(ITrwSerializationWriteContext context, Vector3 value)
        {
            context.Writer.StartArray(TrwValueType.Float32);
            context.Writer.WriteValue().Float32(value.X);
            context.Writer.WriteValue().Float32(value.Y);
            context.Writer.WriteValue().Float32(value.Z);
            context.Writer.EndArray();
        }

        public override Vector3 LoadContent(ITrwSerializationReadContext context)
        {
            context.Reader.CheckAndMoveNext(TrwTokenType.StartArray);
            var x = context.Read<float>();
            var y = context.Read<float>();
            var z = context.Read<float>();
            context.Reader.CheckAndMoveNext(TrwTokenType.EndArray);
            return new Vector3(x, y, z);
        }
    }
}