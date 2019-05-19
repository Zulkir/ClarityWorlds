using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.Fluids
{
    public class LevelSet
    {
        private const int ParticleMaskRadius = 4;

        public IntSize3 Size { get; }
        public float CellSize { get; }

        private double[] phi;
        private double[] backPhi;
        private float[] mass;
        private float[] backMass;
        private NavierStokesCellState[] states;
        private float[] particleMass;
        private bool[] particleMask;

        private readonly object backWriteLock = new object();

        public double[] AllPhi => phi;
        public float[] AllMasses => mass;
        public bool[] AllParticleMasks => particleMask;

        public ref double Phi(int i, int j) => ref phi[j * Size.Width + i];
        public ref double BackPhi(int i, int j) => ref backPhi[j * Size.Width + i];
        public ref float Mass(int i, int j) => ref mass[j * Size.Width + i];
        public ref float BackMass(int i, int j) => ref backMass[j * Size.Width + i];
        public ref NavierStokesCellState State(int i, int j) => ref states[j * Size.Width + i];
        public ref float ParticleMass(int i, int j) => ref particleMass[j * Size.Width + i];
        public ref bool ParticleMask(int i, int j) => ref particleMask[j * Size.Width + i];
        
        public LevelSet(IntSize3 size, float cellSize, INavierStokesGrid grid)
        {
            Size = size;
            CellSize = cellSize;
            phi = new double[size.Volume()];
            backPhi = new double[size.Volume()];
            mass = new float[size.Volume()];
            backMass = new float[size.Volume()];
            states = new NavierStokesCellState[size.Volume()];
            particleMass = new float[size.Volume()];
            particleMask = new bool[size.Volume()];

            for (int j = 0; j < Size.Height - 1; j++)
            for (int i = 0; i < Size.Width - 1; i++)
            {
                var point = new Vector3(i, j, 0) * CellSize;
                State(i, j) = grid.StateAtPoint(point);
            }
        }

        public void TransferPhi(float deltaT, INavierStokesGrid grid)
        {
            /*
            Parallel.For(0, Size.Volume(), index =>
            {
                var j = index / Size.Width;
                var i = index - j * Size.Width;

                if (i < 1 || i >= Size.Width - 1 || j < 1 || j >= Size.Height - 1)
                    return;

                var dpdx = (Phi(i + 1, j) - Phi(i - 1, j)) / (2 * CellSize);
                var dpdy = (Phi(i, j + 1) - Phi(i, j - 1)) / (2 * CellSize);
                var point = CellSize * (new Vector3(i, j, 0) + new Vector3(0.5f, 0.5f, 0.5f));
                var u = grid.GetVelocityAt(point) * deltaT;
                var deltaPhi = -u.X * dpdx - u.Y * dpdy;
                BackPhi(i, j) = Phi(i, j) + deltaPhi;
            });*/

            /*
            for (int j = 1; j < Size.Height - 1; j++)
            for (int i = 1; i < Size.Width - 1; i++)
            {
                var dpdx = (Phi(i + 1, j) - Phi(i - 1, j)) / (2 * CellSize);
                var dpdy = (Phi(i, j + 1) - Phi(i, j - 1)) / (2 * CellSize);
                var point = CellSize * (new Vector3(i, j, 0) + new Vector3(0.5f, 0.5f, 0.5f));
                var u = grid.GetVelocityAt(point) * deltaT;
                var deltaPhi = -u.X * dpdx - u.Y * dpdy;
                BackPhi(i, j) = Phi(i, j) + deltaPhi;
            }*/
            
            
            Array.Clear(backPhi, 0, backPhi.Length);
            var halfVector = new Vector3(0.5f, 0.5f, 0.5f);
            var cornerOffset = CellSize * halfVector;

            Parallel.For(0, Size.Volume(), index =>
            {
                var j = index / Size.Width;
                var i = index - j * Size.Width;

                if (i < 1 || i >= Size.Width - 1 || j < 1 || j >= Size.Height - 1)
                    return;

                var remainingDeltaT = deltaT;
                var point = CellSize * (new Vector3(i, j, 0) + halfVector);

                while (remainingDeltaT > 0)
                {
                    var v = grid.GetVelocityAt(point);
                    var vlen = v.Length();
                    var localDeltaT = vlen * remainingDeltaT > CellSize ? CellSize / vlen : remainingDeltaT;
                    var potentialPoint = point + v * localDeltaT;
                    var potentialCorner = potentialPoint - cornerOffset;
                    var normPotentialCorner = potentialCorner / CellSize;
                    var npci = (int)normPotentialCorner.X;
                    var npcj = (int)normPotentialCorner.Y;
                    if (State(npci, npcj) == NavierStokesCellState.Object)
                        break;
                    point = potentialPoint;
                    remainingDeltaT -= localDeltaT;
                }

                var corner = point - cornerOffset;
                //var centerPoint = new Vector3(Size.Width, Size.Height, 0) * CellSize / 2;
                //while (grid.StateAtPoint(corner) == NavierStokesCellState.Object)
                //{
                //    corner = Vector3.Lerp(corner, centerPoint, 0.05f);
                //}

                var ti = (int)(corner.X / CellSize);
                var tj = (int)(corner.Y / CellSize);
                var intCorner = new Vector3(ti, tj, 0) * CellSize;

                var rightVolume = (corner.X - intCorner.X) / CellSize;
                var upVolume = (corner.Y - intCorner.Y) / CellSize;
                var leftVolume = 1 - rightVolume;
                var downVolume = 1 - upVolume;

                var originalMass = Phi(i, j);
                var a00 = originalMass * leftVolume * downVolume;
                var a01 = originalMass * leftVolume * upVolume;
                var a10 = originalMass * rightVolume * downVolume;
                var a11 = originalMass * rightVolume * upVolume;

                lock (backWriteLock)
                {
                    BackPhi(ti, tj) += a00;
                    BackPhi(ti + 1, tj) += a10;
                    BackPhi(ti, tj + 1) += a01;
                    BackPhi(ti + 1, tj + 1) += a11;
                }
            });
            
            CodingHelper.Swap(ref phi, ref backPhi);
        }

        public void InitPhi()
        {
            for (int j = 0; j < Size.Height; j++)
            for (int i = 0; i < Size.Width; i++)
            {
                var currMass = Mass(i, j) + ParticleMass(i, j);
                Phi(i, j) = currMass > 0f ? -float.MaxValue : float.MaxValue;
            }

            var positiveBorderPoints = new List<Vector2>();
            var negativeBorderPoints = new List<Vector2>();

            for (int j = 1; j < Size.Height - 1; j++)
            for (int i = 1; i < Size.Width - 1; i++)
            {
                var curr = Phi(i, j);
                var left = Phi(i - 1, j);
                var right = Phi(i + 1, j);
                var down = Phi(i, j - 1);
                var up = Phi(i, j + 1);
                if (curr > 0 && (left < 0 || right < 0 || down < 0 || up < 0))
                    positiveBorderPoints.Add(PointForCoords(i, j).Xy);
                else if (curr < 0 && (left > 0 || right > 0 || down > 0 || up > 0))
                    negativeBorderPoints.Add(PointForCoords(i, j).Xy);
            }

            Parallel.For(0, Size.Width * Size.Height, index =>
            {
                var j = index / Size.Width;
                var i = (index - j * Size.Width);

                var currPoint = PointForCoords(i, j).Xy;
                var distSq = float.MaxValue;

                if (Phi(i, j) > 0)
                {
                    foreach (var point in negativeBorderPoints)
                    {
                        var newLength = (currPoint - point).LengthSquared();
                        if (newLength < distSq)
                            distSq = newLength;
                    }
                    Phi(i, j) = Math.Sqrt(distSq);
                }
                else
                {
                    foreach (var point in positiveBorderPoints)
                    {
                        var newLength = (currPoint - point).LengthSquared();
                        if (newLength < distSq)
                            distSq = newLength;
                    }
                    Phi(i, j) = -Math.Sqrt(distSq);
                }
            });

            /*
            for (int j = 1; j < Size.Height - 1; j++)
            for (int i = 1; i < Size.Width - 1; i++)
            {
                ref var curr = ref Phi(i, j);
                var prevI = Phi(i - 1, j);
                var nextI = Phi(i + 1, j);
                var prevJ = Phi(i, j - 1);
                var nextJ = Phi(i, j + 1);
                if (curr == largestDist)
                {
                    if (prevI < 0 || nextI < 0 || prevJ < 0 || nextJ < 0)
                        curr = 1;
                }
                else if (curr == -largestDist)
                {
                    if (prevI > 0 || nextI > 0 || prevJ > 0 || nextJ > 0)
                        curr = -1;
                }
            }

            for (int j = 1; j < Size.Height - 1; j++)
            for (int i = 1; i < Size.Width - 1; i++)
            {
                ref var curr = ref Phi(i, j);
                var prevI = Phi(i - 1, j);
                var nextI = Phi(i + 1, j);
                var prevJ = Phi(i, j - 1);
                var nextJ = Phi(i, j + 1);
                if (curr == largestDist)
                {
                    if (prevI == 1 || nextI == 1 || prevJ == 1 || nextJ == 1)
                        curr = 2;
                }
                else if (curr == -largestDist)
                {
                    if (prevI == -1 || nextI == -1 || prevJ == -1 || nextJ == -1)
                        curr = -2;
                }
            }*/
        }

        public float Curvature(int i, int j)
        {
            var point = CellSize * (new Vector3(i, j, 0) + new Vector3(0.5f, 0.5f, 0.5f));
            return Curvature(point);
        }

        public float Curvature(Vector3 point)
        {
            var gradLeft = GradPhiAt(new Vector3(point.X - CellSize, point.Y, point.Z)).Normalize();
            var gradRight = GradPhiAt(new Vector3(point.X + CellSize, point.Y, point.Z)).Normalize();
            var gradBottom = GradPhiAt(new Vector3(point.X, point.Y - CellSize, point.Z)).Normalize();
            var gradTop = GradPhiAt(new Vector3(point.X, point.Y + CellSize, point.Z)).Normalize();
            var div = gradRight.X - gradLeft.X + gradTop.Y - gradBottom.Y;
            return div;
        }

        public Vector2 GradPhiAt(Vector3 point)
        {
            var phiLeft = PhiAt(new Vector3(point.X - CellSize, point.Y, point.Z));
            var phiRight = PhiAt(new Vector3(point.X + CellSize, point.Y, point.Z));
            var phiBottom = PhiAt(new Vector3(point.X, point.Y - CellSize, point.Z));
            var phiTop = PhiAt(new Vector3(point.X, point.Y + CellSize, point.Z));

            var dpdx = (phiRight - phiLeft) / (2 * CellSize);
            var dpdy = (phiTop - phiBottom) / (2 * CellSize);
            return new Vector2((float)dpdx, (float)dpdy);
        }
        
        public double PhiAt(Vector3 point)
        {
            var normPoint = point / CellSize - new Vector3(0.5f, 0.5f, 0.5f);
            int i = (int)normPoint.X;
            int j = (int)normPoint.Y;
            var phi1 = Phi(i, j);
            var phi2 = Phi(i + 1, j);
            var phi3 = Phi(i, j + 1);
            var phi4 = Phi(i + 1, j + 1);

            var amountI = normPoint.X - i;
            var amountJ = normPoint.Y - j;

            var phi12 = MathHelper.Lerp(phi1, phi2, amountI);
            var phi34 = MathHelper.Lerp(phi3, phi4, amountI);

            return MathHelper.Lerp(phi12, phi34, amountJ);
        }

        public void ClearParticleMask()
        {
            Array.Clear(particleMask, 0, particleMask.Length);
        }

        public bool AddParticleIfHavePlace(Vector3 point)
        {
            CoordsForPoint(point, out var i, out var j);
            return AddParticleIfHavePlace(i, j);
        }

        public bool AddParticleIfHavePlace(int i, int j)
        {
            if (ParticleMask(i, j))
                return false;
            for (int mi = -ParticleMaskRadius + 1; mi < ParticleMaskRadius; mi++)
            for (int mj = -ParticleMaskRadius + 1; mj < ParticleMaskRadius; mj++)
                ParticleMask(i + mi, j + mj) = true;
            return true;
        }

        public Vector3 PointForCoords(int i, int j)
        {
            return CellSize * (new Vector3(i, j, 0) + new Vector3(0.5f, 0.5f, 0.5f));
        }

        public void CoordsForPoint(Vector3 point, out int i, out int j)
        {
            var normPoint = point / CellSize - new Vector3(0.5f, 0.5f, 0.5f);
            i = (int)normPoint.X;
            j = (int)normPoint.Y;
        }
    }
}