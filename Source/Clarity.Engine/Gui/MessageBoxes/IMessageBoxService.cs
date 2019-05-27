namespace Clarity.Engine.Gui.MessageBoxes
{
    public interface IMessageBoxService
    {
        bool? Show(string text, MessageBoxButtons buttons = MessageBoxButtons.Ok, MessageBoxType type = MessageBoxType.Information);
    }
}