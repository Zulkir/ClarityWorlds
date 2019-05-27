using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public abstract class ArrayTrwHandlerBase<TArray, TBuilder, TItem> : TrwSerializationHandlerBase<TArray>
    {
        public override bool ContentIsProperties => false;

        protected abstract TrwValueType TrwValueType { get; }
        protected abstract IEnumerable<TItem> EnumerateItems(TArray array);
        
        protected abstract TBuilder CreateBuilder();
        protected abstract void AddItem(TBuilder builder, TItem value);
        protected abstract TArray Finalize(TBuilder builder);

        public override void SaveContent(ITrwSerializationWriteContext context, TArray value)
        {
            context.Writer.StartArray(TrwValueType);
            foreach (var elem in EnumerateItems(value))
                context.Write(elem);
            context.Writer.EndArray();
        }

        public override TArray LoadContent(ITrwSerializationReadContext context)
        {
            var array = CreateBuilder();
            context.Reader.CheckAndMoveNext(TrwTokenType.StartArray);
            while (context.Reader.TokenType != TrwTokenType.EndArray)
                AddItem(array, context.Read<TItem>());
            context.Reader.CheckAndMoveNext(TrwTokenType.EndArray);
            return Finalize(array);
        }
    }
}