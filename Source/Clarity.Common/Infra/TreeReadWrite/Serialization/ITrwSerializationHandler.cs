using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwSerializationHandler
    {
        Type Type { get; }
        bool ContentIsProperties { get; }
        void SaveContent(ITrwSerializationWriteContext context, object value);
        object LoadContent(ITrwSerializationReadContext context);
    }

    public interface ITrwSerializationHandler<T> : ITrwSerializationHandler
    {
        void SaveContent(ITrwSerializationWriteContext context, T value);
        new T LoadContent(ITrwSerializationReadContext context);
    }
}