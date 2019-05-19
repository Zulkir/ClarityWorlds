using System;

namespace Clarity.Engine.Visualization.Graphics 
{
    [Flags]
    public enum CgModelVisualElementImmutabilityFlags
    {
        None = 0x0,
        Model = 0x1,
        ModelPartIndex = 0x2,
        Material = 0x4,
        Transform = 0x8,
        NonUniformTransform = 0x10,
        TransformSpace = 0x20,
        CullFace = 0x40,
        PolygonMode = 0x80,
        ZOffset = 0x100,
        HighlightEffect = 0x200,
        Hide = 0x400,
    }
}