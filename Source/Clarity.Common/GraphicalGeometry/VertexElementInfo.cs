namespace Clarity.Common.GraphicalGeometry
{
    public class VertexElementInfo : IVertexElementInfo
    {
        public string Semantic { get; }
        public int ArrayIndex { get; }
        public int Offset { get; }
        public int Stride { get; }
        public CommonFormat Format { get; }
        public CommonVertexSemantic CommonSemantic { get; }

        public VertexElementInfo(CommonVertexSemantic semantic, int arrayIndex, CommonFormat format, int offset, int stride)
        {
            CommonSemantic = semantic;
            Semantic = semantic.ToString();
            ArrayIndex = arrayIndex;
            Format = format;
            Offset = offset;
            Stride = stride;
        }

        public VertexElementInfo(string semantic, int arrayIndex, CommonFormat format, int offset, int stride)
        {
            CommonSemantic = CommonVertexSemantic.Undefined;
            Semantic = semantic;
            ArrayIndex = arrayIndex;
            Format = format;
            Offset = offset;
            Stride = stride;
        }
    }
}