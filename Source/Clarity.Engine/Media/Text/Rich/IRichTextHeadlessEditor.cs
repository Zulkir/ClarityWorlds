namespace Clarity.Engine.Media.Text.Rich 
{
    public interface IRichTextHeadlessEditor
    {
        IRichText Text { get; }

        int CursorPos { get; }
        int? SelectionStartPos { get; }
        RtAbsRange? SelectionRange { get; }

        void MoveCursor(int newPos, bool selecting);
        void MoveCursorSafe(int newPos, bool selecting);
        void ClearSelection();

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