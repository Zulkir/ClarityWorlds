using System;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Interaction.Input.VRController
{
    public class VRControllerState : IVRControllerState, ICloneable
    {
        public VRControllerButtons Buttons { get; set; }
        private Transform transform;
        public Vector3 Position { get => transform.Offset; set => transform.Offset = value; }
        public Quaternion Rotation { get => transform.Rotation; set => transform.Rotation = value; }
        public Transform Transform { get => transform; set => transform = value; }
        public Vector3 LeftForward { get => INITIAL_FORWARD * Rotation.ToMatrix3x3();}

        public Vector3 INITIAL_FORWARD = Vector3.UnitY;


        public VRControllerState() { }

        public VRControllerState(VRControllerButtons buttons, Transform transform)
        {
            Buttons = buttons;
            Transform = transform;
        }

        public VRControllerState(VRControllerButtons buttons, Vector3 position, Quaternion rotation)
        {
            Buttons = buttons;
            Transform = new Transform(1, rotation, position);
        }

        public object Clone()
        {
            return new VRControllerState
            {
                Buttons = Buttons,
                Transform = Transform
            };
        }
    }
}
