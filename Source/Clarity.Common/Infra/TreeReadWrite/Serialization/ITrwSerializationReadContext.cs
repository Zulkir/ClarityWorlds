using System;
using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwSerializationReadContext : IDisposable
    {
        ITrwReader Reader { get; }
        IDictionary<string, object> Bag { get; }

        T Read<T>();
        object Read(Type type);

        T ReadFluent<T>(T val);
        Type ReadType();
    }
}