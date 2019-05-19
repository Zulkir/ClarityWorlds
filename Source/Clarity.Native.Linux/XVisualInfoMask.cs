using System;

namespace Clarity.Native.Linux
{
    [Flags]
    public enum XVisualInfoMask
    {
        No = 0x0,
        ID = 0x1,
        Screen = 0x2,
        Depth = 0x4,
        Class = 0x8,
        Red = 0x10,
        Green = 0x20,
        Blue = 0x40,
        ColormapSize = 0x80,
        BitsPerRGB = 0x100,
        All = 0x1FF,
    }
}