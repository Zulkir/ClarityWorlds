using System;

namespace IritNet
{
    [Flags]
    public enum UserMicroTilingType
    {
        USER_MICRO_TILE_REGULAR = 0x1,
        USER_MICRO_TILE_IMPLICIT = 0x2,
        USER_MICRO_TILE_RANDOM = 0x4,

        USER_MICRO_TILE_FORCE_POLY = 0x100,
        USER_MICRO_TILE_FORCE_QUADRATIC = 0x200,
        USER_MICRO_TILE_FORCE_CUBIC = 0x400
    }
}