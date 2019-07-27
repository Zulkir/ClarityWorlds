using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.RayHittables;

namespace Clarity.Engine.Interaction.Input.Mouse
{
    public interface IMouseEvent : IInputEvent
    {
        MouseEventType ComplexEventType { get; }
        MouseButtons EventButtons { get; }
        IMouseState State { get; }
        IntVector2 Delta { get; }
        int WheelDelta { get; }
        KeyModifiers KeyModifiers { get; }
        RayHitResult RayHitResult { get; set; }
    }
}