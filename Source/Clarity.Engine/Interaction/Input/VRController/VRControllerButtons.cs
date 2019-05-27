using System;

namespace Clarity.Engine.Interaction.Input.VRController
{
    [Flags]
    public enum VRControllerButtons
    {
        None = 0x0,
        LeftMenu = 0x1,
        RightMenu = 0x2,
        LeftTrackpad = 0x4,
        RightTrackpad = 0x8,
        LeftTrigger = 0x10,
        RightTrigger = 0x20
    }
}
