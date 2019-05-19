using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class ArrayTrwHandler<T> : TrwSerializationHandlerBase<T[]>
    {
        public override bool ContentIsProperties => false;

        public override void SaveContent(ITrwSerializationWriteContext context, T[] value)
        {
            context.Writer.StartArray(TrwValueType.Undefined);
            foreach (var elem in value)
                context.Write(elem);
            context.Writer.EndArray();
        }

        public override T[] LoadContent(ITrwSerializationReadContext context)
        {
            var list = new List<T>();
            context.Reader.CheckAndMoveNext(TrwTokenType.StartArray);
            while (context.Reader.TokenType != TrwTokenType.EndArray)
                list.Add(context.Read<T>());
            context.Reader.CheckAndMoveNext(TrwTokenType.EndArray);
            return list.ToArray();
        }
    }
}