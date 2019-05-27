using System;

namespace Clarity.Engine.Visualization.Elements 
{
    [Flags]
    public enum ModelVisualElementImmutabilityFlags
    {
        None = 0x0,
        Hide = 0x1,
        Model = 0x2,
        ModelPartIndex = 0x4,
        Material = 0x8,
        RenderState = 0x10,
        Transform = 0x20,
        NonUniformTransform = 0x40,
        TransformSpace = 0x80,
    }
}