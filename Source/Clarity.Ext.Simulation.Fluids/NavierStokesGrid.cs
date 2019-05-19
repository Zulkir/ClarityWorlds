using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.Fluids
{
    public class NavierStokesGrid : INavierStokesGrid
    {
        public NavierStokesCell[] Cells { get; }
        public IntSize3 Size { get; }
        public float CellSize { get; }
        private readonly int spanJ;
        private readonly int spanK;

        public ref NavierStokesCell Cell(int i, int j, int k) => 
            ref Cells[k * spanK + j * spanJ + i];

        public NavierStokesGrid(IntSize3 size, float cellSize)
        {
            Size = size;
            CellSize = cellSize;
            spanJ = size.Width;
            spanK = size.Width * size.Height;
            Cells = new NavierStokesCell[new IntSize3(size.Width + 1, size.Height + 1, size.Depth + 1).Volume()];
        }

        public void DecomposeIndex(int index, out int i, out int j, out int k)
        {
            k = index / spanK;
            var remK = index - k * spanK;
            j = remK / spanJ;
            i = remK - j * spanJ;
        }

        public Vector3 GetVelocityAt(Vector3 point)
        {
            var normPoint = point / CellSize;
            var i = (int)normPoint.X;
            var j = (int)normPoint.Y;
            var k = (int)normPoint.Z;
            var ux = MathHelper.Lerp(Cell(i, j, k).Velocity.X, Cell(i + 1, j, k).Velocity.X, normPoint.X - i);
            var uy = MathHelper.Lerp(Cell(i, j, k).Velocity.Y, Cell(i, j + 1, k).Velocity.Y, normPoint.Y - j);
            var uz = MathHelper.Lerp(Cell(i, j, k).Velocity.Z, Cell(i, j, k + 1).Velocity.Z, normPoint.Z - k);
            return new Vector3(ux, uy, uz);
        }

        public Vector3 GetCenterVelocityAt(int i, int halfI, int j, int halfJ, int k, int halfK)
        {
            var point = CellSize * new Vector3(i + 0.5f * (halfI + 1), j + 0.5f * (halfJ + 1), k + 0.5f * (halfK + 1));
            return GetVelocityAt(point);
        }

        public NavierStokesCellState StateAtPoint(Vector3 point)
        {
            var normPoint = point / CellSize;
            var i = (int)normPoint.X;
            var j = (int)normPoint.Y;
            var k = (int)normPoint.Z;
            if (i < Size.Width && j < Size.Height && k < Size.Depth)
                return Cell(i, j, k).State;
            return NavierStokesCellState.Object;
        }
    }
}