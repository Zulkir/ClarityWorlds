using Clarity.Common.Numericals.OtherTuples;

namespace Clarity.Common.GraphicalGeometry {
    public interface ILineGeometryReader : IGeometryReaderBase
    {
        int NumLineSegments { get; }
        IntVector2 GetLineSegment(int lineSegmentIndex);
    }
}