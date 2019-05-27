using System;
using Clarity.Common.CodingUtilities;

namespace Clarity.App.Worlds.SaveLoad
{
    [Flags]
    public enum SaveWorldFlags
    {
        None = 0,
        EditableWorld = 0b01,
        ReadOnlyWorld = 0b10,
    }

    public static class SaveWorldFlagsExtensions
    {
        public static bool HasFlag(this SaveWorldFlags value, SaveWorldFlags flag) =>
            CodingHelper.HasFlag((int)value, (int)flag);
    }
}