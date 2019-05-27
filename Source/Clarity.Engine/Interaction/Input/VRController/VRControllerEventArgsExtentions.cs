namespace Clarity.Engine.Interaction.Input.VRController
{
    public static class VRControllerEventArgsExtentions
    {
        public static bool IsOfType(this IVRControllerEventArgs eventArgs, VRControllerEventType type) =>
            eventArgs.EventType == type;

        public static bool IsLeftTriggerPress(this IVRControllerEventArgs eventArgs) =>
            eventArgs.IsOfType(VRControllerEventType.Down) && eventArgs.EventButtons == VRControllerButtons.LeftTrigger;
    }
}
