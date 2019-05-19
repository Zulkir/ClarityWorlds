using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Engine.Gui;
using Clarity.Engine.Visualization.Viewports;
using UnityEngine;
using Vector2 = Clarity.Common.Numericals.Algebra.Vector2;

namespace Assets.Scripts.Rendering
{
    public class UcRenderGuiControl : IRenderGuiControl
    {
        public object RenderLibHandle => null;
        public int Width => Screen.width;
        public int Height => Screen.height;
        public IViewport FocusedViewport { get; private set; }
        public IReadOnlyList<IViewport> Viewports { get; private set; }

        public IReadOnlyList<IGuiCommand> Commands => commands;
        public IViewport MainViewport => Viewports?.FirstOrDefault();

        private List<IGuiCommand> commands;
        private ViewportsLayout tableLayout;

        public void SetViewports(IReadOnlyList<IViewport> viewports, ViewportsLayout layout)
        {
            tableLayout = layout;
            Viewports = viewports;
            FocusedViewport = MainViewport;
            ResizeViewports();
        }

        private void ResizeViewports()
        {
            var numRows = tableLayout.ViewportIndices.GetLength(0);
            var numCols = tableLayout.ViewportIndices.GetLength(1);
            var offsets = new Vector2[numRows + 1, numCols + 1];
            var rowOffset = 0f;
            for (int r = 0; r < numRows + 1; r++)
            {
                var colOffset = 0f;
                for (int c = 0; c < numCols + 1; c++)
                {
                    offsets[r, c] = new Vector2(colOffset, rowOffset);
                    if (c < numCols)
                        colOffset += ToActualPixels(tableLayout.ColumnWidths[c], Width);
                }
                if (r < numRows)
                    rowOffset += ToActualPixels(tableLayout.RowHeights[r], Height);
            }

            for (int i = 0; i < Viewports.Count; i++)
            {
                var viewport = Viewports[i];
                int startRow, endRow, startCol, endCol;
                if (TryGetViewportBorderCells(i, out startRow, out endRow, out startCol, out endCol))
                {
                    var topLeft = offsets[startRow, startCol];
                    var bottomRight = offsets[endRow + 1, endCol + 1];
                    var size = bottomRight - topLeft;
                    viewport.OnResized(topLeft.X, topLeft.Y, size.X, size.Y);
                }
                else
                {
                    viewport.OnResized(0, 0, 0, 0);
                }
            }
        }

        private static float ToActualPixels(ViewportLength guiLength, int pixelLength)
        {
            switch (guiLength.Unit)
            {
                case ViewportLengthUnit.Pixels:
                case ViewportLengthUnit.ScaledPixels: return guiLength.Value;
                case ViewportLengthUnit.Percent: return guiLength.Value * pixelLength / 100;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private bool TryGetViewportBorderCells(int viewportIndex, out int startRow, out int endRow, out int startCol, out int endCol)
        {
            startRow = startCol = int.MaxValue;
            endRow = endCol = int.MinValue;

            var numRows = tableLayout.ViewportIndices.GetLength(0);
            var numCols = tableLayout.ViewportIndices.GetLength(1);
            for (int r = 0; r < numRows; r++)
                for (int c = 0; c < numCols; c++)
                {
                    if (tableLayout.ViewportIndices[r, c] != viewportIndex)
                        continue;
                    if (startRow > r)
                        startRow = r;
                    if (startCol > c)
                        startCol = c;
                    if (endRow < r)
                        endRow = r;
                    if (endCol < c)
                        endCol = c;
                }

            return startRow != int.MaxValue && startCol != int.MaxValue;
        }

        public void SetCommands(IReadOnlyList<IGuiCommand> commands)
        {
            this.commands.Clear();
            this.commands.AddRange(commands);
        }
    }
}