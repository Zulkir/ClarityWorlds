using System;
using Clarity.Common.CodingUtilities;

namespace Clarity.Engine.Interaction.Input.Mouse
{
    [Flags]
    public enum MouseButtons
    {
        None = 0x0,
        Left = 0x1,
        Right = 0x2,
        Middle = 0x4
    }

    public static class MouseButtonsExtensions
    {
        public static bool HasFlag(this MouseButtons buttons, MouseButtons flag) => CodingHelper.HasFlag((int)buttons, (int)flag);
    }
}