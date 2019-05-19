using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class TransformTrwHandler : TrwSerializationHandlerBase<Transform>
    {
        public override bool ContentIsProperties => true;

        public override void SaveContent(ITrwSerializationWriteContext context, Transform value)
        {
            context.WriteProperty("Scale", value.Scale);
            context.WriteProperty("Rotation", value.Rotation);
            context.WriteProperty("Offset", value.Offset);
        }

        public override Transform LoadContent(ITrwSerializationReadContext context)
        {
            Transform result;
            context.Reader.MoveNext();
            result.Scale = context.Read<float>();
            context.Reader.MoveNext();
            result.Rotation = context.Read<Quaternion>();
            context.Reader.MoveNext();
            result.Offset = context.Read<Vector3>();
            return result;
        }
    }
}