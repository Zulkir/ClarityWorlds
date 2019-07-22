namespace Clarity.Engine.Interaction.Input
{
    public interface IInputLock
    {
        InputEventProcessResult ProcessEvent(IInputEvent args);
    }
}