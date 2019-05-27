using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Interaction.Input.VRController
{
    public interface IVRControllerState
    {
        VRControllerButtons Buttons { get; set; }
        Transform Transform { get; set; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 LeftForward { get;}
    }
}
