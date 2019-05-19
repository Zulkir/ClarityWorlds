using System.Collections.Generic;

namespace Clarity.Core.AppCore.StoryGraph.Editing.Flowchart
{
    public class FlowchartGrid
    {
        private readonly List<List<int>> cols;

        public IReadOnlyList<IReadOnlyList<int>> Cols => cols;

        public FlowchartGrid()
        {
            cols = new List<List<int>>();
        }

        public List<int> GetCol(int rowIndex)
        {
            while (cols.Count <= rowIndex)
                cols.Add(new List<int>());
            return cols[rowIndex];
        }
    }
}