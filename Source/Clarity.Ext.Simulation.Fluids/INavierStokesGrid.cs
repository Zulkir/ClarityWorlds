using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.Fluids
{
    public interface INavierStokesGrid
    {
        IntSize3 Size { get; }
        float CellSize { get; }
        NavierStokesCell[] Cells { get; }
        ref NavierStokesCell Cell(int i, int j, int k);
        void DecomposeIndex(int index, out int i, out int j, out int k);

        Vector3 GetVelocityAt(Vector3 point);
        Vector3 GetCenterVelocityAt(int i, int halfI, int j, int halfJ, int k, int halfK);
        NavierStokesCellState StateAtPoint(Vector3 point);
    }
}