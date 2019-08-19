using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public interface ICirclePackingBorder
    {
        AaRectangle2 BoundingRect { get; }
        float Area { get; }
        IReadOnlyList<Vector2> Points { get; }
        Vector2 FindClosestValidPoint(Vector2 point);
        bool PointIsValid(Vector2 point);
    }
}