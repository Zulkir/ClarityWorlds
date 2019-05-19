using JetBrains.Annotations;

namespace Clarity.Engine.Resources.RawData
{
    public struct RawDataResSubrange
    {
        [NotNull]
        public IRawDataResource RawDataResource;
        public int StartOffset;
        public int Length;

        public RawDataResSubrange([NotNull] IRawDataResource rawDataResource, int startOffset)
        {
            RawDataResource = rawDataResource;
            StartOffset = startOffset;
            Length = rawDataResource.Size - startOffset;
        }

        public RawDataResSubrange([NotNull] IRawDataResource rawDataResource, int startOffset, int length)
        {
            RawDataResource = rawDataResource;
            StartOffset = startOffset;
            Length = length;
        }
    }
}