using System;

namespace Clarity.Native.Win32
{
    [Flags]
    public enum MK
    {
        LBUTTON = 0x0001,
        RBUTTON = 0x0002,
        SHIFT = 0x0004,
        CONTROL = 0x0008,
        MBUTTON = 0x0010,
        XBUTTON1 = 0x0020,
        XBUTTON2 = 0x0040
    }
}
