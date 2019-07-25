namespace Clarity.Engine.Media.Text.Rich 
{
    public interface IRichTextHeadlessEditor
    {
        int CursorAbsPosition { get; set; }
        RtAbsRange? SelectionRange { get; set; }
        void InputString(string str);
        void Erase();
        void Tab();
        void ShiftTab();

        bool CanCopy();
        string Copy();
        string Cut();
        void Paste(string str);
    }
}