using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public abstract class TrwSerializationHandlerBase<T> : ITrwSerializationHandler<T>
    {
        public Type Type => typeof(T);

        void ITrwSerializationHandler.SaveContent(ITrwSerializationWriteContext context, object value) => SaveContent(context, (T)value);
        object ITrwSerializationHandler.LoadContent(ITrwSerializationReadContext context) => LoadContent(context);

        public abstract bool ContentIsProperties { get; }
        public abstract void SaveContent(ITrwSerializationWriteContext context, T value);
        public abstract T LoadContent(ITrwSerializationReadContext context);
    }
}