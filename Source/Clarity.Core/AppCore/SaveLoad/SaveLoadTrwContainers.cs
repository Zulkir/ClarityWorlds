using System;

namespace Clarity.Core.AppCore.SaveLoad
{
    [Flags]
    public enum SaveLoadTrwContainers
    {
        None = 0,
        Basic = 0x1,
        World = 0x2
    }
}