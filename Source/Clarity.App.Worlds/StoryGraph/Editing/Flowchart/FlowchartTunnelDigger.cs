using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.App.Worlds.StoryGraph.Editing.Flowchart
{
    public class FlowchartTunnelDigger
    {
        private readonly Dictionary<int, FlowchartGrid> grids;
        private readonly Size2[] sizes;
        private readonly Vector2[] positions;
        private readonly IStoryGraph sg;
        private readonly int[] nodeCols;

        public IStoryGraph Graph => sg;

        private static readonly Size2 LeafSize = new Size2(2, 2);
        private static readonly float Margin = 0.5f;

        public FlowchartTunnelDigger(IStoryGraph graph)
        {
            sg = graph;
            grids = graph.NonLeaves.ToDictionary(x => x, x => new FlowchartGrid());
            sizes = new Size2[graph.EnoughIntegers.Count];
            positions = new Vector2[graph.EnoughIntegers.Count];
            nodeCols = sg.EnoughIntegers.Select(x => -1).ToArray();
        }

        private int GetNodeRow(int node)
        {
            if (nodeCols[node] == -1)
                nodeCols[node] = CalcNodeRow(node);
            return nodeCols[node];
        }

        private int CalcNodeRow(int node)
        {
            var siblingPrevs = sg.Children[sg.Parents[node]].Where(x => sg.LeveledNext[x].Contains(node));
            var result = siblingPrevs.Select(GetNodeRow).ConcatSingle(-1).Max() + 1;
            nodeCols[node] = result;
            return result;
        }

        private Size2 GetNodeSize(int node)
        {
            if (sizes[node] == default(Size2))
                sizes[node] = CalcNodeSize(node);
            return sizes[node];
        }

        private Size2 CalcNodeSize(int node)
        {
            if (!sg.Children[node].Any())
                return LeafSize;
            var grid = grids[node];

            var totalWidth = 0f;
            var totalHeight = 0f;
            foreach (var col in grid.Cols)
            {
                var colWidth = 0f;
                var colHeight = 0f;
                foreach (var child in col)
                {
                    var childSize = GetNodeSize(child);
                    if (childSize.Width > colWidth)
                        colWidth = childSize.Width;
                    colHeight += 2 * Margin + childSize.Height;
                }

                if (colHeight > totalHeight)
                    totalHeight = colHeight;
                totalWidth += colWidth + 2 * Margin;
            }

            var offsetToCenter = new Vector2(-totalWidth, -totalHeight) / 2;

            var wOffset = Margin;
            foreach (var col in grid.Cols)
            {
                var colWidth = col.Max(x => GetNodeSize(x).Width);
                var localHeightMargin = (totalHeight - col.Sum(x => GetNodeSize(x).Height)) / (col.Count + 1);
                var hOffset = localHeightMargin;
                foreach (var child in col)
                {
                    var childSize = GetNodeSize(child);
                    var localWidthMargin = (colWidth - childSize.Width) / 2;
                    positions[child] = 
                        new Vector2(wOffset + localWidthMargin, hOffset) + 
                        new Vector2(childSize.Width, childSize.Height) / 2 +
                        offsetToCenter;
                    hOffset += childSize.Height + localHeightMargin;
                }
                
                wOffset += colWidth + 2 * Margin;
            }

            return new Size2(totalWidth, totalHeight);
        }


        public AaRectangle2[] Dig()
        {
            if (sg.Children[sg.Root].Any())
                DigGreedy(sg.Root);
            Optimize();
            return BuildRectangles();
        }

        private void DigGreedy(int node)
        {
            var grid = grids[node];
            foreach (var child in sg.Children[node])
                grid.GetCol(GetNodeRow(child)).Add(child);
            foreach (var child in sg.Children[node].Where(x => sg.Children[x].Any()))
                DigGreedy(child);
        }

        private void Optimize()
        {
            // todo
        }

        private AaRectangle2[] BuildRectangles()
        {
            foreach (var node in sg.NodeIds)
                GetNodeSize(node);
            return sg.EnoughIntegers.Select(x =>
            {
                if (!sg.IsUsed[x])
                    return new AaRectangle2();
                var pos = positions[x];
                var size = sizes[x];
                return new AaRectangle2(pos, size.Width / 2, size.Height / 2);
            }).ToArray();
        }
    }
}