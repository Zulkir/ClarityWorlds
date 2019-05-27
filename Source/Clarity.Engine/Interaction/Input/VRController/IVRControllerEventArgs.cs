using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Interaction.Input.VRController
{
    public interface IVRControllerEventArgs : IInputEventArgs
    {
        VRControllerEventType EventType { get; }
        VRControllerButtons EventButtons { get; }
        Vector3 Delta { get; }
        IVRControllerState State { get; }
    }
}
