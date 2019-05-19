namespace Clarity.Engine.Gui
{
    public interface IClipboardService
    {
        void CopyString(string str);
        bool TryGetString(out string str);
    }
}