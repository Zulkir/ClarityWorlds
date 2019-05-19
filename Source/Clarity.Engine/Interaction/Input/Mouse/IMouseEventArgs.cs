using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Interaction.Input.Keyboard;

namespace Clarity.Engine.Interaction.Input.Mouse
{
    public interface IMouseEventArgs : IInputEventArgs
    {
        MouseEventType ComplexEventType { get; }
        MouseButtons EventButtons { get; }
        IMouseState State { get; }
        IntVector2 Delta { get; }
        int WheelDelta { get; }
        KeyModifyers KeyModifyers { get; }
    }
}