using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.App.Worlds.Misc.HighlightOnMouse
{
    public class HighlightOnMouseService : IHighlightOnMouseService
    {
        private readonly Dictionary<string, List<object>> activeObjectsByGroup;

        public IEnumerable<string> HighlightedGroups => activeObjectsByGroup
            .Where(x => x.Value.HasItems())
            .Select(x => x.Key);

        public HighlightOnMouseService()
        {
            activeObjectsByGroup = new Dictionary<string, List<object>>();
        }

        public bool GroupIsHighlighted(string groupName)
        {
            return activeObjectsByGroup.TryGetValue(groupName, out var list) && list.HasItems();
        }

        public void OnObjectIn(object obj, string groupName)
        {
            activeObjectsByGroup.GetOrAdd(groupName, x => new List<object>()).Add(obj);
        }

        public void OnObjectOut(object obj, string groupName)
        {
            activeObjectsByGroup.GetOrAdd(groupName, x => new List<object>()).Remove(obj);
        }
    }
}