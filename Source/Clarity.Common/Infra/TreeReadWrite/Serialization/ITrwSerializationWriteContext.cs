using System;
using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwSerializationWriteContext : IDisposable
    {
        ITrwWriter Writer { get; }
        IDictionary<string, object> Bag { get; }
        IDictionary<Type, string> TypeAliases { get; set; }

        void Write<T>(T value);
        void Write(Type type, object value);
        void WriteType(Type value);
    }
}