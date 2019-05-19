using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.OtherTuples;

namespace Clarity.Engine.Interaction.Input.Mouse
{
    public interface IMouseState
    {
        MouseButtons Buttons { get; }
        IntVector2 Position { get; }
        Vector2 NormalizedPosition { get; }
        Vector2 HmgnPosition { get; }
        int WheelPosition { get; }
    }
}