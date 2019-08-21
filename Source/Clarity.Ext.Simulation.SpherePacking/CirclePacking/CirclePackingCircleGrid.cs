using System;
using System.Collections.Generic;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;

namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public class CirclePackingCircleGrid
    {
        private const float CellSizeInRadii = 2.1f;
        private const int MaxCirclesPerCell = 16;
        
        private readonly int[] cellCircleCounts;
        private readonly int[] circleIndices;

        private readonly float cellSize;
        private readonly IntSize2 gridSize;
        private readonly Vector2 minCorner;

        private Vector2[] circleCenters;

        public CirclePackingCircleGrid(float circleRadius, AaRectangle2 boundingRect)
        {
            cellSize = CellSizeInRadii * circleRadius;
            var gridWidth = (int)MathHelper.Ceiling(boundingRect.Width / cellSize);
            var gridHeight = (int)MathHelper.Ceiling(boundingRect.Height / cellSize);
            gridSize = new IntSize2(gridWidth, gridHeight);
            cellCircleCounts = new int[gridSize.Area];
            circleIndices = new int[gridSize.Area * MaxCirclesPerCell];
            minCorner = boundingRect.MinMin;
        }

        private IntVector2 CellVectorIndex(Vector2 point) => 
            new IntVector2((point - minCorner) / cellSize);

        private int CellIndexFor(Vector2 point) => 
            CellIndexFromVectorIndex(CellVectorIndex(point));

        private int CellIndexFromVectorIndex(IntVector2 vectorIndex) => 
            vectorIndex.Y * gridSize.Width + vectorIndex.X;

        private static int CircleIndicesOffsetForCell(int cellIndex) => 
            cellIndex * MaxCirclesPerCell;

        public void Reset()
        {
            circleCenters = null;
            Array.Clear(cellCircleCounts, 0, cellCircleCounts.Length);
        }

        public bool TryFit(Vector2 point)
        {
            var cellIndex = CellIndexFor(point);
            if (cellCircleCounts[cellIndex] >= MaxCirclesPerCell) 
                return false;
            cellCircleCounts[cellIndex]++;
            return true;
        }

        public void Rebuild(Vector2[] circleCenters, int numCircles)
        {
            this.circleCenters = circleCenters;
            Array.Clear(cellCircleCounts, 0, cellCircleCounts.Length);
            for (var i = 0; i < numCircles; i++)
                AddIndexToCell(CellIndexFor(circleCenters[i]), i);
        }

        private void AddIndexToCell(int cellIndex, int index)
        {
            var indexInCell = cellCircleCounts[cellIndex];
            if (indexInCell >= MaxCirclesPerCell)
                throw new ArgumentOutOfRangeException();
            var indicesOffset = CircleIndicesOffsetForCell(cellIndex);
            circleIndices[indicesOffset + indexInCell] = index;
            cellCircleCounts[cellIndex]++;
        }

        public IEnumerable<int> GetNeighborIndices(int index)
        {
            var cellVectorIndex = CellVectorIndex(circleCenters[index]);
            for (var ix = Math.Max(0, cellVectorIndex.X - 1); ix <= Math.Min(cellVectorIndex.X + 1, gridSize.Width - 1); ix++)
            for (var iy = Math.Max(0, cellVectorIndex.Y - 1); iy <= Math.Min(cellVectorIndex.Y + 1, gridSize.Height - 1); iy++)
            {
                var cellIndex = CellIndexFromVectorIndex(new IntVector2(ix, iy));
                var offset = CircleIndicesOffsetForCell(cellIndex);
                for (var i = 0; i < cellCircleCounts[cellIndex]; i++)
                    if (circleIndices[offset + i] != index)
                        yield return circleIndices[offset + i];
            }
        }
    }
}