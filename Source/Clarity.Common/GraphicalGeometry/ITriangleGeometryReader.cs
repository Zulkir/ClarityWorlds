using Clarity.Common.Numericals.OtherTuples;

namespace Clarity.Common.GraphicalGeometry {
    public interface ITriangleGeometryReader : IGeometryReaderBase
    {
        int NumTriangles { get; }
        IntVector3 GetTriangle(int triangleIndex);
    }
}