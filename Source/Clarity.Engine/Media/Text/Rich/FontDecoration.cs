using System;

namespace Clarity.Engine.Media.Text.Rich
{
    [Flags]
    public enum FontDecoration
    {
        None = 0,
        Bold = 0x1,
        Italic = 0x2,
        Underline = 0x4,
        Strikethrough = 0x8,
    }

    public static class FontDecorationExtensions
    {
        public static bool HasFlags(this FontDecoration value, FontDecoration flags) => 
            (value & flags) == flags;
    }
}