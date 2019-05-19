using System;

namespace Clarity.Engine.Interaction.Input
{
    [Flags]
    public enum InputEventProcessResult
    {
        DontCare = 0,
        StopPropagating = 0x1,
        ReleaseLock = 0x2
    }
}