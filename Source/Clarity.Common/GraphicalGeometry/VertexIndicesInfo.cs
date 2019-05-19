namespace Clarity.Common.GraphicalGeometry
{
    public class VertexIndicesInfo : IVertexIndicesInfo
    {
        public int ArrayIndex { get; }
        public CommonFormat Format { get; }
        
        public VertexIndicesInfo(int arrayIndex, CommonFormat format)
        {
            ArrayIndex = arrayIndex;
            Format = format;
        }
    }
}