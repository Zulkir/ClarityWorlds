using System;
using Clarity.Common.CodingUtilities;

namespace Clarity.Engine.Interaction.Input.Keyboard
{
    // todo: fix spelling

    [Flags]
    public enum KeyModifyers
    {
        None = 0x0,
        Control = 0x1,
        Shift = 0x2,
        Alt = 0x4
    }

    public static class KeyModifyersExtensionMethods
    {
        public static bool HasFlag(this KeyModifyers modifyers, KeyModifyers flag) =>
            CodingHelper.HasFlag((int)modifyers, (int)flag);
    }
}