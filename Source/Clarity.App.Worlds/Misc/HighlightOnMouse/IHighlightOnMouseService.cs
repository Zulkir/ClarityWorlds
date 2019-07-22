using System.Collections.Generic;

namespace Clarity.App.Worlds.Misc.HighlightOnMouse
{
    public interface IHighlightOnMouseService
    {
        IEnumerable<string> HighlightedGroups { get; }
        bool GroupIsHighlighted(string groupName);

        void OnObjectIn(object obj, string groupName);
        void OnObjectOut(object obj, string groupName);
    }
}