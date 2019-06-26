namespace Clarity.Engine.Interaction.Input.Keyboard
{
    public interface IKeyEvent : IInputEvent
    {
        KeyEventType ComplexEventType { get; }
        Key EventKey { get; }
        IKeyboardState State { get; }
        KeyModifyers KeyModifyers { get; }
        string Text { get; }
        bool HasFocus { get; }
    }
}