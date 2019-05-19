using System;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public interface ITrwWriter : IDisposable
    {
        void AddProperty(string name);
        void StartObject();
        void EndObject();
        void StartArray(TrwValueType arrayType);
        void EndArray();
        ITrwValueWriter WriteValue();
        void Flush();

        string NextEntryPath { get; }
    }
}