using System;

namespace Clarity.Engine.Visualization.Elements.Materials
{
    [Flags]
    public enum StandardMaterialImmutabilityFlags
    {
        None = 0x0,
        HasTransparency = 0x1,
        DiffuseColor = 0x2,
        DiffuseMap = 0x4,
        NormalMap = 0x8,
        Sampler = 0x10,
        IgnoreLighting = 0x20,
        NoSpecular = 0x40,
        HighlightEffect = 0x80,
        RtTransparencyMode = 0x100,
        All = HasTransparency |
              DiffuseColor |
              DiffuseMap |
              NormalMap |
              Sampler |
              IgnoreLighting |
              NoSpecular |
              HighlightEffect |
              RtTransparencyMode
    }
}