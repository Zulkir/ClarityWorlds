using System;

namespace Clarity.Engine.Gui.WindowQueries
{
    public interface IWindowQueryService
    {
        bool TryQueryText(string windowTitle, string initialText, out string text);
        void QueryTextMutable(string windowTitle, string initialText, Action<string> onTextChanged);
    }
}