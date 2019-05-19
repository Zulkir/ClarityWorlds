using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class PairTrwHandler<T> : TrwSerializationHandlerBase<Pair<T>>
    {
        public override bool ContentIsProperties => false;

        public override void SaveContent(ITrwSerializationWriteContext context, Pair<T> value)
        {
            context.Writer.StartArray(TrwValueType.Undefined);
            context.Write(value.First);
            context.Write(value.Second);
            context.Writer.EndArray();
        }

        public override Pair<T> LoadContent(ITrwSerializationReadContext context)
        {
            context.Reader.Check(TrwTokenType.StartArray);
            context.Reader.MoveNext();
            var first = context.Read<T>();
            var second = context.Read<T>();
            context.Reader.Check(TrwTokenType.EndArray);
            context.Reader.MoveNext();
            return new Pair<T>(first, second);
        }
    }
}