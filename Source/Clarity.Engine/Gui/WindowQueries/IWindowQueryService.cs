using System;

namespace Clarity.Engine.Gui.WindowQueries
{
    public interface IWindowQueryService
    {
        void QueryTextMutable(string windowTitle, string initialText, Action<string> onTextChanged);
    }
}