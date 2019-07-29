using System;
using Clarity.Common.CodingUtilities;

namespace Clarity.Engine.Interaction.Input.Keyboard
{
    // todo: fix spelling

    [Flags]
    public enum KeyModifiers
    {
        None = 0x0,
        Control = 0x1,
        Shift = 0x2,
        Alt = 0x4
    }

    public static class KeyModifiersExtensionMethods
    {
        public static bool HasFlag(this KeyModifiers modifiers, KeyModifiers flag) =>
            CodingHelper.HasFlag((int)modifiers, (int)flag);
    }
}