namespace Clarity.Engine.Interaction.Input.Keyboard
{
    public interface IKeyEventArgs : IInputEventArgs
    {
        KeyEventType ComplexEventType { get; }
        Key EventKey { get; }
        IKeyboardState State { get; }
        KeyModifyers KeyModifyers { get; }
        string Text { get; }
        bool HasFocus { get; }
    }
}