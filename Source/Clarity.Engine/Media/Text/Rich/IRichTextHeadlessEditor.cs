using System;

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
        void InsertEmbedding(string embeddingType, string str);
        void Erase();
        void Tab();
        void ShiftTab();

        bool CanCopy();
        string Copy();
        string Cut();
        void Paste(string str);

        bool TryGetParaStyleProp<T>(Func<IRtParagraphStyle, T> getProp, out T prop) where T : IEquatable<T>;
        bool TryGetSpanStyleProp<T>(Func<IRtSpanStyle, T> getProp, out T prop) where T : IEquatable<T>;

        void SetParaStyleProp<T>(T prop, Action<IRtParagraphStyle, T> setProp);
        void SetSpanStyleProp<T>(T prop, Action<IRtSpanStyle, T> setProp);
    }
}