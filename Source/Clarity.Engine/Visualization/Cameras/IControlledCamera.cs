using Clarity.Engine.Interaction.Input;

namespace Clarity.Engine.Visualization.Cameras
{
    public interface IControlledCamera : ICamera
    {
        bool TryHandleInput(IInputEventArgs eventArgs);
    }
}