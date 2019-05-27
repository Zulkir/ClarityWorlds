using System;

namespace Clarity.Engine.Visualization.Elements.RenderStates
{
    [Flags]
    public enum StandardRenderStateImmutabilityFlags
    {
        None = 0x0,
        CullFace = 0x1,
        PolygonMode = 0x2,
        ZOffset = 0x4,
        PointSize = 0x8,
        LineWidth = 0x10,

        All =  CullFace | 
               PolygonMode |
               ZOffset |
               PointSize |
               LineWidth
    }
}