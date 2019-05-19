namespace Clarity.Common.GraphicalGeometry
{
    public interface IPointGeometryReader : IGeometryReaderBase
    {
        int NumPoints { get; }
        int GetPoint(int pointIndex);
    }
}