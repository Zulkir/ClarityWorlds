namespace Clarity.Engine.Interaction.Input.VRController
{
    public static class VRControllerEventArgsExtentions
    {
        public static bool IsOfType(this IVrControllerEvent eventArgs, VRControllerEventType type) =>
            eventArgs.EventType == type;

        public static bool IsLeftTriggerPress(this IVrControllerEvent eventArgs) =>
            eventArgs.IsOfType(VRControllerEventType.Down) && eventArgs.EventButtons == VRControllerButtons.LeftTrigger;
    }
}
