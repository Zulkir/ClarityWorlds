using System;
using Clarity.Common.CodingUtilities;

namespace Clarity.Common.Infra.ActiveModel
{
    [Flags]
    public enum AmBindingFlags
    {
        None = 0x0,
        Reference = 0x1,
        Derived = 0x2
    }

    public static class AmBindingFlagsExtensions
    {
        public static bool HasFlag(this AmBindingFlags value, AmBindingFlags flag) => CodingHelper.HasFlag((int)value, (int)flag);
    }
}