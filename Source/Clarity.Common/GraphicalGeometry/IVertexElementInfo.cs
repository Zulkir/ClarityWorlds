namespace Clarity.Common.GraphicalGeometry
{
    // todo: move to CgModel folder?
    public interface IVertexElementInfo
    {
        string Semantic { get; }
        int ArrayIndex { get; }
        int Offset { get; }
        int Stride { get; }
        CommonFormat Format { get; }
        CommonVertexSemantic CommonSemantic { get; }
    }
}