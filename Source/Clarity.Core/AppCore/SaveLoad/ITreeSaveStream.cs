using System;

namespace Clarity.Core.AppCore.SaveLoad
{
    public interface ITreeSaveStream : IDisposable
    {
        void BeginElement(string name);
        void EndElement();

        void AddNullAttribute();
        void AddAttribute(string name, Type value);
        void AddAttribute(string name, string value);

        void FillElementValue(string value);
        void FillElementValue(Type value);
        void FillElementValue(bool value);
        void FillElementValue(int value);
        void FillElementValue(float value);
        void FillElementValue(float[] value);
    }
}