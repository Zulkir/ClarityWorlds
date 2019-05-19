using System;
using System.IO;

namespace Clarity.Engine.Resources.RawData
{
    public interface IRawDataResource : IResource
    {
        int Size { get; }
        Stream Open();
        IntPtr Map();
        void Unmap(bool wasModified);
        IRawDataResourceDisposableMap MapToDisposable(bool willModify);
    }

    public static class RawDataResourceImmediateExtensions
    {
        public static RawDataResSubrange AsSubrange(this IRawDataResource dataRes) =>
            new RawDataResSubrange(dataRes, 0);

        public static RawDataResSubrange GetSubrange(this IRawDataResource dataRes, int startOffset) => 
            new RawDataResSubrange(dataRes, startOffset);

        public static RawDataResSubrange GetSubrange(this IRawDataResource dataRes, int startOffset, int length) =>
            new RawDataResSubrange(dataRes, startOffset, length);
    }
}