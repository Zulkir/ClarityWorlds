using System;
using Clarity.Common.CodingUtilities;

namespace Assets.Scripts.Interaction
{
    [Flags]
    public enum VrInputDispatcherCapabilities
    {
        None = 0b00,
        SwitchingModes = 0b01,
        Minimap = 0b10,
        All = 0b11
    }

    public static class VrInputDispatcherCapabilitiesExtensions
    {
        public static bool HasFlag(this VrInputDispatcherCapabilities value, VrInputDispatcherCapabilities flag)
        {
            return CodingHelper.HasFlag((int)value, (int)flag);
        }
    }
}