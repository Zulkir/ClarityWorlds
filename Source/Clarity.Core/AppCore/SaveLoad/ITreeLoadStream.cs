using System;

namespace Clarity.Core.AppCore.SaveLoad
{
    public interface ITreeLoadStream : IDisposable
    {
        bool IsEndOfChildren { get; }

        void MoveDown();
        void MoveNext();
        void MoveUp();
        
        string Name { get; }
        string RawValue { get; }

        string LoadString();
        Type LoadType();
        bool LoadBool();
        int LoadInt();
        float LoadFloat();
        float[] LoadFloatArray();
        void Skip();
    }
}