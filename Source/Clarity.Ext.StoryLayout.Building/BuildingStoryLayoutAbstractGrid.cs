using System.Collections.Generic;

namespace Clarity.Ext.StoryLayout.Building
{
    public class BuildingStoryLayoutAbstractGrid
    {
        private readonly List<List<int>> rows;

        public int RowCount => rows.Count;

        public BuildingStoryLayoutAbstractGrid()
        {
            rows = new List<List<int>>();
        }

        public IEnumerable<List<int>> EnumerateRows() => rows;

        public List<int> GetRow(int rowIndex)
        {
            while (rows.Count <= rowIndex)
                rows.Add(new List<int>());
            return rows[rowIndex];
        }
    }
}