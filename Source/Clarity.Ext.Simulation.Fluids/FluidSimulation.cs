using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Clarity.App.Worlds.External.FluidSimulation;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.Fluids
{
    public class FluidSimulation : IFluidSimulation
    {
        private const float ParticleCreationCurvature = 1f;
        private const float ParticleRemovalPhi = -0.1f;

        private ConcurrentQueue<FluidSimulationFrame> frameQueue;

        private INavierStokesGrid currentNavierStokesGrid;
        private INavierStokesGrid nextNavierStokesGrid;
        private INavierStokesGrid backNavierStokesGrid;
        private FluidParticle[] particles;
        private IntSize3 size;
        private float cellSize;
        private int numCells;
        private int spanJ;
        private int spanK;
        private Vector3 gravityForce;
        private float atmosphericPressure;
        private float density;
        private LevelSet levelSet;
        private float viscosity;
        private Random random;
        private bool stop;
        private int levelSetScale;
        private FluidSurfaceType surfaceType;
        private Task processingTask;

        //private float[] rk;
        //private float[] rk1;
        //private float[] pk;
        //private float[] pk1;
        //private float[] apk;
        //private float[] xk;
        //private float[] xk1;

        public int NumCells => numCells;
        public INavierStokesGrid CurrentNavierStokesGrid => currentNavierStokesGrid;
        public FluidParticle[] Particles => particles;
        public ConcurrentQueue<FluidSimulationFrame> FrameQueue => frameQueue;
        public LevelSet LevelSet => levelSet;

        public FluidSimulation(float atmosphericPressure, float density, float viscosity)
        {
            this.atmosphericPressure = atmosphericPressure;
            this.density = density;
            this.viscosity = viscosity;
            random = new Random();
            particles = new FluidParticle[1 << 16];
            size = new IntSize3(20, 20, 1);
            levelSetScale = 8;
            cellSize = 0.8f;
            currentNavierStokesGrid = new NavierStokesGrid(size, cellSize);
            levelSet = new LevelSet(new IntSize3(size.Width * levelSetScale, size.Height * levelSetScale, 1), cellSize / levelSetScale, currentNavierStokesGrid);
        }

        public void Reset(FluidSimulationConfig config)
        {
            stop = true;
            processingTask?.Wait();
            processingTask = null;

            size = new IntSize3(config.Width, config.Height, 1);
            cellSize = config.CellSize;
            surfaceType = config.SurfaceType;
            //this.particleRadius = particleRadius;
            
            frameQueue = new ConcurrentQueue<FluidSimulationFrame>();
            
            currentNavierStokesGrid = new NavierStokesGrid(size, cellSize);
            nextNavierStokesGrid = new NavierStokesGrid(size, cellSize);
            backNavierStokesGrid = new NavierStokesGrid(size, cellSize);
            numCells = size.Volume();
            spanJ = size.Width;
            spanK = size.Width * size.Height;
            //rk = new float[numCells];
            //rk1 = new float[numCells];
            //pk = new float[numCells];
            //pk1 = new float[numCells];
            //apk = new float[numCells];
            //xk = new float[numCells];
            //xk1 = new float[numCells];
            gravityForce = new Vector3(0, -0.98f * 3, 0) / cellSize * 0.4f;
            InitBorderCellStates();
            levelSetScale = config.LevelSetScale;
            levelSet = new LevelSet(new IntSize3(size.Width * levelSetScale, size.Height * levelSetScale, 1), cellSize / levelSetScale, currentNavierStokesGrid);
            SetInitialMass();
            particles = new FluidParticle[1 << 16];
            GenerateInitialParticles();
            if (surfaceType != FluidSurfaceType.Particles)
                levelSet.InitPhi();
        }

        private void SetInitialMass()
        {
            for (int j = (int)(0.2f * levelSet.Size.Height); j < 0.85f * levelSet.Size.Height; j++)
            for (int i = (int)(0.6f * levelSet.Size.Width); i < 0.8f * levelSet.Size.Width; i++)
            {
                levelSet.Mass(i, j) = 1f;
            }
            //levelSet.RecalculatePhi();
        }

        private void GenerateInitialParticles()
        {
            /*
            var resultList = new List<Vector3>();
            var dropletRadius = Math.Min(Math.Min(size.Width, size.Height), size.Depth) * cellSize / 4;
            var dropletRadiusSq = dropletRadius * dropletRadius;
            var dropletCenter = cellSize * new Vector3(size.Width, size.Height, size.Depth) / 2;
            for (int k = 0; k < size.Depth * 4; k++)
            for (int j = 4; j < size.Height * 4 - 4; j++)
            for (int i = 0; i < size.Width * 4; i++)
            {
                var point = cellSize * new Vector3(i, j, k) / 4;
                //var point = cellSize * (new Vector3(i, j, k) + new Vector3(5, 5, 5)) / 20;
                //if ((point - dropletCenter).LengthSquared() < dropletRadiusSq)
                    resultList.Add(point);
            }
            return resultList.ToArray();*/

            int c = 0;

            int k = 0;
            //for (int k = 0; k < size.Depth; k++)
            for (int j = 0; j < levelSet.Size.Height; j++)
            for (int i = 0; i < levelSet.Size.Width; i++)
            {
                if (levelSet.Mass(i, j) == 0)
                    continue;
                if (surfaceType != FluidSurfaceType.Particles || i % 4 != 0 || j % 4 != 0)
                    continue;

                var point = levelSet.CellSize * new Vector3(i, j, k);
                //for (int k = 0; k < size.Depth; k++)
                //for (int j2 = 1; j2 < 3; j2++)
                //for (int i2 = 1; i2 < 3; i2++)
                //{
                //    resultList.Add(point + levelSet.CellSize * new Vector3(i2, j2, 2) / 3);
                //}

                particles[c].Position = point + levelSet.CellSize * new Vector3(1, 1, 1) / 2;

                c++;
            }

            //            
            //var dropletRadius = Math.Min(Math.Min(size.Width, size.Height), size.Depth) * cellSize / 4;
            //var dropletRadiusSq = dropletRadius * dropletRadius;
            //var dropletCenter = cellSize * new Vector3(size.Width, size.Height, size.Depth) / 2;
            //for (int k = 1; k < 2 /*size.Depth * 4*/; k++)
            //for (int j = 8 + 16; j < size.Height * 8 - 8; j++)
            //for (int i = 8 + 16; i < size.Width * 8 - 16; i++)
            //{
            //    var point = cellSize * new Vector3(i, j, k) / 8;
            //    //var point = cellSize * (new Vector3(i, j, k) + new Vector3(5, 5, 5)) / 20;
            //    //if ((point - dropletCenter).LengthSquared() < dropletRadiusSq)
            //    resultList.Add(point);
            //}
        }

        private void InitBorderCellStates()
        {
            for (int k = 0; k < size.Depth; k++)
            for (int j = 0; j < size.Height; j++)
            {
                currentNavierStokesGrid.Cell(0, j, k).State = NavierStokesCellState.Object;
                currentNavierStokesGrid.Cell(1, j, k).State = NavierStokesCellState.Object;
                currentNavierStokesGrid.Cell(currentNavierStokesGrid.Size.Width - 1, j, k).State = NavierStokesCellState.Object;
                currentNavierStokesGrid.Cell(currentNavierStokesGrid.Size.Width - 2, j, k).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(0, j, k).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(1, j, k).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(nextNavierStokesGrid.Size.Width - 1, j, k).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(nextNavierStokesGrid.Size.Width - 2, j, k).State = NavierStokesCellState.Object;
                backNavierStokesGrid.Cell(0, j, k).State = NavierStokesCellState.Object;
                backNavierStokesGrid.Cell(1, j, k).State = NavierStokesCellState.Object;
                backNavierStokesGrid.Cell(nextNavierStokesGrid.Size.Width - 1, j, k).State = NavierStokesCellState.Object;
                backNavierStokesGrid.Cell(nextNavierStokesGrid.Size.Width - 2, j, k).State = NavierStokesCellState.Object;
            }

            for (int k = 0; k < size.Depth; k++)
            for (int i = 0; i < size.Width; i++)
            {
                currentNavierStokesGrid.Cell(i, 0, k).State = NavierStokesCellState.Object;
                currentNavierStokesGrid.Cell(i, 1, k).State = NavierStokesCellState.Object;
                currentNavierStokesGrid.Cell(i, currentNavierStokesGrid.Size.Height - 1, k).State = NavierStokesCellState.Object;
                currentNavierStokesGrid.Cell(i, currentNavierStokesGrid.Size.Height - 2, k).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(i, 0, k).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(i, 1, k).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(i, nextNavierStokesGrid.Size.Height - 1, k).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(i, nextNavierStokesGrid.Size.Height - 2, k).State = NavierStokesCellState.Object;
                backNavierStokesGrid.Cell(i, 0, k).State = NavierStokesCellState.Object;
                backNavierStokesGrid.Cell(i, 1, k).State = NavierStokesCellState.Object;
                backNavierStokesGrid.Cell(i, nextNavierStokesGrid.Size.Height - 1, k).State = NavierStokesCellState.Object;
                backNavierStokesGrid.Cell(i, nextNavierStokesGrid.Size.Height - 2, k).State = NavierStokesCellState.Object;
            }

            /*
            for (int j = 0; j < size.Height; j++)
            for (int i = 0; i < size.Width; i++)
            {
                currentNavierStokesGrid.Cell(i, j, 0).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(i, j, 0).State = NavierStokesCellState.Object;
                currentNavierStokesGrid.Cell(i, j, currentNavierStokesGrid.Size.Depth - 1).State = NavierStokesCellState.Object;
                nextNavierStokesGrid.Cell(i, j, currentNavierStokesGrid.Size.Depth - 1).State = NavierStokesCellState.Object;
            }*/
        }

        public void Run(float initialTimestamp)
        {
            stop = false;
            if (frameQueue == null)
                throw new Exception();
            processingTask = Task.Run(() => SimulationLoop(initialTimestamp));
        }

        public void Stop()
        {
            stop = true;
        }

        public void SimulationLoop(float initialTimestamp)
        {
            var lastFrameTimestamp = initialTimestamp;
            var currentTimestamp = initialTimestamp;
            while (!stop)
            {
                var frameSize = sizeof(float) * 3 * particles.Length + 9 * levelSet.AllPhi.Length;
                var maxFrames = (1 << 29) / frameSize;
                while (!stop && frameQueue.Count > maxFrames)
                    Thread.Sleep(1);

                if (stop)
                    break;

                var maxVel = Enumerable.Range(0, numCells).Select(x => nextNavierStokesGrid.Cells[x].Velocity.Length()).Max();
                var deltaT = Math.Min(0.5f * cellSize / maxVel, 1 / 10f);
                AdvanceSimulation(deltaT);
                currentTimestamp += deltaT;
                if (currentTimestamp - lastFrameTimestamp > 1f/20)
                {
                    lastFrameTimestamp = currentTimestamp;
                    var newFrame = new FluidSimulationFrame(levelSet.Size, particles.Length, nextNavierStokesGrid.Size, nextNavierStokesGrid.CellSize)
                    {
                        Timestamp = currentTimestamp
                    };
                    for (int i = 0; i < particles.Length; i++)
                    {
                        newFrame.Particles[i] = particles[i].Position;
                    }
                    for (int i = 0; i < levelSet.Size.Volume(); i++)
                    {
                        newFrame.Phi[i] = levelSet.AllPhi[i];
                        newFrame.ParticleMask[i] = levelSet.AllParticleMasks[i];
                    }
                    for (int i = 0; i < numCells; i++)
                        newFrame.NavierStokesGrid.Cells[i] = nextNavierStokesGrid.Cells[i];
                    frameQueue.Enqueue(newFrame);
                }
            }
        }

        private void AdvanceSimulation(float deltaT)
        {
            var remainingT = deltaT;

            const float maxmaxvel = 500;
            const float maxvel = 10;
            const float veldumping = 0.9f;

            while (remainingT > 0)
            {
                var maxVel = Enumerable.Range(0, numCells).Select(x => currentNavierStokesGrid.Cells[x].Velocity.Length()).Max();
                if (maxVel > maxmaxvel)
                    for (int i = 0; i < numCells; i++)
                        currentNavierStokesGrid.Cells[i].Velocity *= (maxmaxvel / maxVel);
                if (maxVel > maxvel)
                    for (int i = 0; i < numCells; i++)
                        currentNavierStokesGrid.Cells[i].Velocity *= veldumping;
                maxVel = Enumerable.Range(0, numCells).Select(x => currentNavierStokesGrid.Cells[x].Velocity.Length()).Max();

                var maxDist = 0.5f * cellSize;
                var minDeltaT = 0.001f;
                var smallDeltaT = Math.Max(Math.Min(maxDist / maxVel, remainingT), minDeltaT);
                
                UpdateVelocities2(smallDeltaT);

                maxVel = Enumerable.Range(0, numCells).Select(x => nextNavierStokesGrid.Cells[x].Velocity.Length()).Max();
                if (maxVel > maxmaxvel)
                    for (int i = 0; i < numCells; i++)
                        nextNavierStokesGrid.Cells[i].Velocity *= (maxmaxvel / maxVel);
                if (maxVel > maxvel)
                    for (int i = 0; i < numCells; i++)
                        nextNavierStokesGrid.Cells[i].Velocity *= veldumping;
                
                EnforceMassConservation4(smallDeltaT);
                MoveParticles(smallDeltaT);
                if (surfaceType != FluidSurfaceType.Particles)
                    MovePhi(smallDeltaT);
                if (surfaceType == FluidSurfaceType.Hybrid)
                    CreateAndRemoveParticles(smallDeltaT);
                PostUpdateCellStates();
                for (int i = 0; i < numCells; i++)
                    currentNavierStokesGrid.Cells[i] = nextNavierStokesGrid.Cells[i];
                remainingT -= smallDeltaT;
            }
            
            //CodingHelper.Swap(ref currentNavierStokesGrid, ref nextNavierStokesGrid);
        }

        private void UpdateVelocities2(float deltaT)
        {
            var invDeltaX = 1f / cellSize;
            var invDeltaY = 1f / cellSize;
            var invDeltaZ = 1f / cellSize;
            var invSqDeltaX = 1f / (cellSize * cellSize);
            var invSqDeltaY = 1f / (cellSize * cellSize);
            var invSqDeltaZ = 1f / (cellSize * cellSize);

            int k = 0;
            //for (int k = 0; k < size.Depth; k++)
            for (int j = 2; j < size.Height - 2; j++)
            for (int i = 2; i < size.Width - 2; i++)
            {
                ref var curr = ref nextNavierStokesGrid.Cell(i, j, k);
                {
                    var v_mh_mh_mh = currentNavierStokesGrid.Cell(i, j, k).Velocity;
                    var v_z_z_z = currentNavierStokesGrid.GetCenterVelocityAt(i, 0, j, 0, k, 0);
                    var v_m_z_z = currentNavierStokesGrid.GetCenterVelocityAt(i - 1, 0, j, 0, k, 0);
                    var v_mh_mh_z = currentNavierStokesGrid.GetCenterVelocityAt(i, -1, j, -1, k, 0);
                    var v_mh_ph_z = currentNavierStokesGrid.GetCenterVelocityAt(i, -1, j, +1, k, 0);
                    //var v_mh_z_mh = currentNavierStokesGrid.GetCenterVelocityAt(i, -1, j, 0, k, -1);
                    //var v_mh_z_ph = currentNavierStokesGrid.GetCenterVelocityAt(i, -1, j, 0, k, +1);
                    var v_ph_z_z = currentNavierStokesGrid.GetCenterVelocityAt(i, +1, j, 0, k, 0);
                    var v_mh_z_z = currentNavierStokesGrid.GetCenterVelocityAt(i, -1, j, 0, k, 0);
                    var v_mt_z_z = currentNavierStokesGrid.GetCenterVelocityAt(i, -3, j, 0, k, 0);
                    var v_mh_m_z = currentNavierStokesGrid.GetCenterVelocityAt(i, -1, j - 1, 0, k, 0);
                    var p_m_z_z = currentNavierStokesGrid.Cell(i - 1, j, k).Pressure;
                    var p_z_z_z = currentNavierStokesGrid.Cell(i, j, k).Pressure;

                    var prev = v_mh_mh_mh.X;
                    var a = invDeltaX * (v_m_z_z.X * v_m_z_z.X - v_z_z_z.X * v_z_z_z.X);
                    var b = invDeltaY * (v_mh_mh_z.X * v_mh_mh_z.Y - v_mh_ph_z.X * v_mh_ph_z.Y);
                    //var c = invDeltaZ * (v_mh_z_mh.X * v_mh_z_mh.Z - v_mh_z_ph.X * v_mh_z_ph.Z);
                    var c = 0f;
                    var d = invDeltaX * (p_m_z_z - p_z_z_z);
                    var e = 0f;
                    var f = invSqDeltaX * (v_ph_z_z.X - 2 * v_mh_z_z.X + v_mt_z_z.X);
                    var g = invSqDeltaY * (v_mh_ph_z.X - 2 * v_mh_z_z.X + v_mh_m_z.X);
                    var h = 0f;

                    //a = b = 0;
                    //d = 0;

                    var delta = deltaT * (a + b + c + d + e + viscosity * (f + g + h));
                    //if (delta > 100)
                    //    delta = 100;
                    //if (delta < -100)
                    //    delta = -100;
                    curr.Velocity.X = prev + delta;
                }
                {
                    var v_mh_mh_mh = currentNavierStokesGrid.Cell(i, j, k).Velocity;
                    var v_mh_mh_z = currentNavierStokesGrid.GetCenterVelocityAt(i, -1, j, -1, k, 0);
                    var v_ph_mh_z = currentNavierStokesGrid.GetCenterVelocityAt(i, +1, j, -1, k, 0);
                    var v_z_m_z = currentNavierStokesGrid.GetCenterVelocityAt(i, 0, j - 1, 0, k, 0);
                    var v_z_z_z = currentNavierStokesGrid.GetCenterVelocityAt(i, 0, j, 0, k, 0);
                    var v_z_mh_z = currentNavierStokesGrid.GetCenterVelocityAt(i, 0, j, -1, k, 0);
                    var v_z_mt_z = currentNavierStokesGrid.GetCenterVelocityAt(i, 0, j, -3, k, 0);
                    var v_z_ph_z = currentNavierStokesGrid.GetCenterVelocityAt(i, 0, j, +1, k, 0);
                    var p_z_m_z = currentNavierStokesGrid.Cell(i, j - 1, k).Pressure;
                    var p_z_z_z = currentNavierStokesGrid.Cell(i, j, k).Pressure;

                    var prev = v_mh_mh_mh.Y;
                    var a = invDeltaX * (v_mh_mh_z.Y * v_mh_mh_z.X - v_ph_mh_z.Y * v_ph_mh_z.X);
                    var b = invDeltaY * (v_z_m_z.Y * v_z_m_z.Y - v_z_z_z.Y * v_z_z_z.Y);
                    var c = 0f;
                    var d = invDeltaY * (p_z_m_z - p_z_z_z);
                    var e = curr.State == NavierStokesCellState.Liquid ? gravityForce.Y : 0;
                    //var e = gravityForce.Y;
                    var f = invSqDeltaX * (v_ph_mh_z.Y - 2 * v_z_mh_z.Y + v_mh_mh_z.Y);
                    var g = invSqDeltaY * (v_z_ph_z.Y - 2 * v_z_mh_z.Y + v_z_mt_z.Y);
                    var h = 0f;

                    //a = b = 0;
                    //d = 0;

                    var delta = deltaT * (a + b + c + d + e + viscosity * (f + g + h));
                    curr.Velocity.Y = prev + delta;
                }
            }
        }
        
        private void AddForce(Vector3 force, float deltaT)
        {
            for (int i = 0; i < numCells; i++)
                if (nextNavierStokesGrid.Cells[i].State == NavierStokesCellState.Liquid)
                    nextNavierStokesGrid.Cells[i].Velocity = nextNavierStokesGrid.Cells[i].Velocity + force * deltaT;
        }
        
        private void EnforceMassConservationForSurface()
        {
            var k = 0;

            //for (int k = 1; k < size.Depth - 1; k++)
            for (int j = 1; j < size.Height - 1; j++)
            for (int i = 1; i < size.Width - 1; i++)
            {
                ref var curr = ref nextNavierStokesGrid.Cell(i, j, k);
                ref var prevI = ref nextNavierStokesGrid.Cell(i - 1, j, k);
                ref var nextI = ref nextNavierStokesGrid.Cell(i + 1, j, k);
                ref var prevJ = ref nextNavierStokesGrid.Cell(i, j - 1, k);
                ref var nextJ = ref nextNavierStokesGrid.Cell(i, j + 1, k);
                //ref var prevK = ref nextNavierStokesGrid.Cell(i, j, k - 1);
                //ref var nextK = ref nextNavierStokesGrid.Cell(i, j, k + 1);

                if (curr.State != NavierStokesCellState.Liquid)
                    continue;

                if (prevI.State == NavierStokesCellState.Object)
                    curr.Velocity.X = 0;
                if (nextI.State == NavierStokesCellState.Object)
                    nextI.Velocity.X = 0;
                if (prevJ.State == NavierStokesCellState.Object)
                    curr.Velocity.Y = 0;
                if (nextJ.State == NavierStokesCellState.Object)
                    nextJ.Velocity.Y = 0;

                var numEmptyNeighbors = 0;
                var inflow = 0f;

                if (prevI.State == NavierStokesCellState.Empty)
                    numEmptyNeighbors++;
                if (nextI.State == NavierStokesCellState.Empty)
                    numEmptyNeighbors++;
                if (prevJ.State == NavierStokesCellState.Empty)
                    numEmptyNeighbors++;
                if (nextJ.State == NavierStokesCellState.Empty)
                    numEmptyNeighbors++;
                
                if (numEmptyNeighbors == 0)
                    continue;

                curr.IsSurface = true;
                curr.Pressure = atmosphericPressure;

                //if (prevI.State == NavierStokesCellState.Liquid)
                    inflow += curr.Velocity.X;
                //if (nextI.State == NavierStokesCellState.Liquid)
                    inflow -= nextI.Velocity.X;
                //if (prevJ.State == NavierStokesCellState.Liquid)
                    inflow += curr.Velocity.Y;
                //if (nextJ.State == NavierStokesCellState.Liquid)
                    inflow -= nextJ.Velocity.Y;

                var inflowCompensation = inflow / numEmptyNeighbors;

                if (prevI.State == NavierStokesCellState.Empty)
                    curr.Velocity.X -= inflowCompensation;
                if (nextI.State == NavierStokesCellState.Empty)
                    nextI.Velocity.X += inflowCompensation;
                if (prevJ.State == NavierStokesCellState.Empty)
                    curr.Velocity.Y -= inflowCompensation;
                if (nextJ.State == NavierStokesCellState.Empty)
                    nextJ.Velocity.Y += inflowCompensation;
            }
        }

        private void EnforceMassConservationForSubmerged(float deltaT)
        {
            var coeff = density * cellSize / deltaT;
            /*
            // rk = b = b - Ax0
            for (int i = 0; i < numCells; i++)
            {
                var cell = nextNavierStokesGrid.Cells[i];
                if (cell.State != NavierStokesCellState.Liquid || cell.IsSurface)
                    continue;
                var nui = i < numCells - 1 ? nextNavierStokesGrid.Cells[i + 1].Velocity.X : 0f;
                var nuj = i < numCells - spanJ ? nextNavierStokesGrid.Cells[i + spanJ].Velocity.Y : 0f;
                var nuk = i < numCells - spanK ? nextNavierStokesGrid.Cells[i + spanK].Velocity.Z : 0f;
                var grad = nui - cell.Velocity.X + nuj - cell.Velocity.Y + nuk - cell.Velocity.Z;
                rk[i] = coeff * grad;
            }

            // pk = rk
            Array.Copy(rk, pk, pk.Length);

            for (int k = 0; k < numCells; k++)
            {
                var alphakNum = Dot(rk, rk);
                MultByA(pk, apk);
                var alphakDenom = Dot(pk, apk);
                if (Math.Abs(alphakDenom) < 0.0001f)
                    break;

                var alphak = alphakNum / alphakDenom;
                for (int i = 0; i < numCells; i++)
                {
                    xk1[i] = xk[i] + alphak * pk[i];
                    rk1[i] = rk[i] - alphak * apk[i];
                }
                // if rk1 is small enough, break
                var betakNum = Dot(rk1, rk1);
                var betakDenom = Dot(rk, rk);
                var betak = betakNum / betakDenom;
                for (int i = 0; i < numCells; i++)
                    pk1[i] = rk1[i] + betak * pk[i];
            }*/

            //for (int i = 0; i < numCells; i++)
            //{
            //    if (float.IsNaN(xk1[i]) || float.IsInfinity(xk1[i]))
            //    {
            //        int x = 0;
            //    }
            //    //nextNavierStokesGrid.Cells[i].Pressure = MathHelper.Clamp(xk1[i], atmosphericPressure, 10 * atmosphericPressure);
            //    //nextNavierStokesGrid.Cells[i].Pressure = xk1[i];
            //    nextNavierStokesGrid.Cells[i].Pressure = atmosphericPressure;
            //}

            //foreach (var particle in particles)
            //{
            //    var i = (int)(particle.X / cellSize);
            //    var j = (int)(particle.Y / cellSize);
            //    var k = (int)(particle.Z / cellSize);
            //    nextNavierStokesGrid.Cell(i, j, k).Pressure += atmosphericPressure / 50;
            //}

            for (int j = 2; j < size.Height - 2; j++)
            for (int i = 2; i < size.Width - 2; i++)
                {
                nextNavierStokesGrid.Cell(i, j, 0).Pressure = atmosphericPressure * Math.Min(Math.Max(MathHelper.Pow(AvgLevelSetMassFor(i, j), 3), 1), 3);
            }

            var velCoeff = deltaT / (density * cellSize);

            var k_ = 0;
            //for (int k = 1; k < size.Depth - 1; k++)
            for (int j = 1; j < size.Height - 1; j++)
            for (int i = 1; i < size.Width - 1; i++)
            {
                ref var curr = ref nextNavierStokesGrid.Cell(i, j, k_);
                //if (curr.State != NavierStokesCellState.Liquid || curr.IsSurface)
                //        continue;
                var prevI = nextNavierStokesGrid.Cell(i - 1, j, k_);
                var prevJ = nextNavierStokesGrid.Cell(i, j - 1, k_);
                //var prevK = nextNavierStokesGrid.Cell(i, j, k_ - 1);
                if (prevI.State != NavierStokesCellState.Object)
                    curr.Velocity.X -= velCoeff * (curr.Pressure - prevI.Pressure);
                if (prevJ.State != NavierStokesCellState.Object)
                    curr.Velocity.Y -= velCoeff * (curr.Pressure - prevJ.Pressure);
                //curr.Velocity.Z -= velCoeff * (curr.Pressure - prevK.Pressure);
            }

            //for (int i = 0; i < numCells; i++)
            //    nextNavierStokesGrid.Cells[i].Velocity *= 0.995f;
        }

        private void EnforceMassConservation2(float deltaT)
        {
            var invDeltaX = 1f / cellSize;
            var invDeltaY = 1f / cellSize;
            var invDeltaZ = 1f / cellSize;
            var invSqDeltaX = 1f / (cellSize * cellSize);
            var invSqDeltaY = 1f / (cellSize * cellSize);
            var invSqDeltaZ = 1f / (cellSize * cellSize);

            var beta0 = 1.1f;
            var laplacian = invSqDeltaX + invSqDeltaY + invSqDeltaZ;

            int badCells = int.MaxValue;

            while (badCells > 0)
            {
                badCells = 0;

                int k = 0;
                //for (int k = 0; k < size.Depth; k++)
                for (int j = 2; j < size.Height - 2; j++)
                for (int i = 2; i < size.Width - 2; i++)
                {
                    ref var curr = ref nextNavierStokesGrid.Cell(i, j, k);
                    ref var nextI = ref nextNavierStokesGrid.Cell(i + 1, j, k);
                    ref var nextJ = ref nextNavierStokesGrid.Cell(i, j + 1, k);
                    var div = -(invDeltaX * (nextI.Velocity.X - curr.Velocity.X) +
                                invDeltaY * (nextJ.Velocity.Y - curr.Velocity.Y) +
                                0f);
                    if (Math.Abs(div) < 0.0001f)
                        continue;
                    
                    var beta = beta0 / (2 * deltaT * laplacian);
                    var deltaP = beta * div;

                    var deltaV = deltaT / cellSize * deltaP;
                    if (float.IsInfinity(deltaV) || float.IsNaN(deltaV))
                        continue;

                    var maxVel = Math.Max(Math.Max(Math.Max(
                        Math.Abs(curr.Velocity.X), Math.Abs(curr.Velocity.Y)), Math.Abs(nextI.Velocity.X)), Math.Abs(nextJ.Velocity.Y));

                    if (Math.Abs(deltaV) / maxVel < 0.000001f)
                        continue;

                    badCells++;

                    curr.Velocity.X -= deltaV;
                    nextI.Velocity.X += deltaV;
                    curr.Velocity.Y -= deltaV;
                    nextJ.Velocity.Y += deltaV;
                    
                    curr.Pressure += deltaP;
                }
            }
        }

        private void EnforceMassConservation3(float deltaT)
        {
            var k = 0;

            for (int j = 2; j < size.Height - 2; j++)
            for (int i = 2; i < size.Width - 2; i++)
            {
                //nextNavierStokesGrid.Cell(i, j, 0).Pressure = atmosphericPressure * nextNavierStokesGrid.Cell(i, j, k).Mass;
                nextNavierStokesGrid.Cell(i, j, 0).Pressure = atmosphericPressure * Math.Min(Math.Max(MathHelper.Pow(AvgLevelSetMassFor(i, j), 3), 1), 100);
                //nextNavierStokesGrid.Cell(i, j, 0).Pressure = atmosphericPressure * Math.Max(AvgLevelSetMassFor(i, j), 1);
            }

            var velCoeff = deltaT / (density * cellSize);

            
            //for (int k = 1; k < size.Depth - 1; k++)
            for (int j = 1; j < size.Height - 1; j++)
            for (int i = 1; i < size.Width - 1; i++)
            {
                ref var curr = ref nextNavierStokesGrid.Cell(i, j, k);
                //if (curr.State != NavierStokesCellState.Liquid)
                //    continue;
                var prevI = nextNavierStokesGrid.Cell(i - 1, j, k);
                var prevJ = nextNavierStokesGrid.Cell(i, j - 1, k);
                if (prevI.State != NavierStokesCellState.Object)
                    curr.Velocity.X -= velCoeff * (curr.Pressure - prevI.Pressure);
                if (prevJ.State != NavierStokesCellState.Object)
                    curr.Velocity.Y -= velCoeff * (curr.Pressure - prevJ.Pressure);
            }
        }

        private volatile bool hasBadCells;

        private void EnforceMassConservation4(float deltaT)
        {
            var invDeltaX = 1f / cellSize;
            var invDeltaY = 1f / cellSize;
            var invDeltaZ = 1f / cellSize;
            var invSqDeltaX = 1f / (cellSize * cellSize);
            var invSqDeltaY = 1f / (cellSize * cellSize);
            var invSqDeltaZ = 1f / (cellSize * cellSize);

            var beta0 = 1.1f;
            var laplacian = invSqDeltaX + invSqDeltaY + invSqDeltaZ;

            //int badCells = int.MaxValue;
            hasBadCells = true;

            while (hasBadCells)
            {
                hasBadCells = false;

                var sizeVolume = size.Volume();
                for (int i = 0; i < sizeVolume; i++)
                {
                    backNavierStokesGrid.Cells[i] = nextNavierStokesGrid.Cells[i];
                }

                //int k = 0;
                //for (int k = 0; k < size.Depth; k++)
                Parallel.For(0, size.Volume(), index =>
                {
                    var decodedIndex = index;
                    var k = decodedIndex / spanK;
                    decodedIndex -= k * spanK;
                    var j = decodedIndex / spanJ;
                    decodedIndex -= j * spanJ;
                    var i = decodedIndex;

                    if (i < 1 || i >= size.Width - 1 || j < 1 || j >= size.Height - 1)
                        return;

                    ref var curr = ref nextNavierStokesGrid.Cells[index];

                    if (curr.State != NavierStokesCellState.Liquid)
                        return;

                    ref var prevI = ref nextNavierStokesGrid.Cells[index - 1];
                    ref var nextI = ref nextNavierStokesGrid.Cells[index + 1];
                    ref var prevJ = ref nextNavierStokesGrid.Cells[index - spanJ];
                    ref var nextJ = ref nextNavierStokesGrid.Cells[index + spanJ];
                    var div = (curr.Velocity.X - nextI.Velocity.X) +
                              (curr.Velocity.Y - nextJ.Velocity.Y) +
                              0f;
                    if (Math.Abs(div) < 0.01f)
                        return;

                    var deltaP = cellSize * 0.001f * div / deltaT;
                    //var deltaP = 0.5f * div / deltaT;
                    var deltaV = 0.1f * div;

                    if (float.IsInfinity(deltaV) || float.IsNaN(deltaV))
                        throw new Exception();

                    //var maxVel = Math.Max(Math.Max(Math.Max(
                    //    Math.Abs(curr.Velocity.X), Math.Abs(curr.Velocity.Y)), Math.Abs(nextI.Velocity.X)), Math.Abs(nextJ.Velocity.Y));
                    //if (Math.Abs(deltaV) / maxVel < 0.000001f)
                    //    return;


                    hasBadCells = true;

                    ref var backCurr = ref backNavierStokesGrid.Cells[index];


                    if (prevI.State == NavierStokesCellState.Object)
                    {
                        backCurr.Velocity.X = 0;
                        backNavierStokesGrid.Cells[index - 1].Velocity.Y = backCurr.Velocity.Y;
                    }
                    else if (prevI.State == NavierStokesCellState.Empty)
                        backCurr.Velocity.X -= 3 * deltaV;
                    else
                        backCurr.Velocity.X -= deltaV;

                    if (nextI.State == NavierStokesCellState.Object)
                    {
                        backNavierStokesGrid.Cells[index + 1].Velocity.X = 0;
                        backNavierStokesGrid.Cells[index + 1].Velocity.Y = backCurr.Velocity.Y;
                    }
                    else if (nextI.State == NavierStokesCellState.Empty)
                        backNavierStokesGrid.Cells[index + 1].Velocity.X += 3 * deltaV;
                    else
                        backNavierStokesGrid.Cells[index + 1].Velocity.X += deltaV;

                    if (prevJ.State == NavierStokesCellState.Object)
                    {
                        backCurr.Velocity.Y = 0;
                        backNavierStokesGrid.Cells[index - spanJ].Velocity.X = backCurr.Velocity.X;
                    }
                    else if (prevJ.State == NavierStokesCellState.Empty)
                        backCurr.Velocity.Y -= 3 * deltaV;
                    else
                        backCurr.Velocity.Y -= deltaV;

                    if (nextJ.State == NavierStokesCellState.Object)
                    {
                        backNavierStokesGrid.Cells[index + spanJ].Velocity.Y = 0;
                        backNavierStokesGrid.Cells[index + spanJ].Velocity.X = backCurr.Velocity.X;
                    }
                    else if (nextJ.State == NavierStokesCellState.Empty)
                        backNavierStokesGrid.Cells[index + spanJ].Velocity.Y += 3 * deltaV;
                    else
                        backNavierStokesGrid.Cells[index + spanJ].Velocity.Y += deltaV;

                    //backNavierStokesGrid.Cell(i, j, k).Velocity.X += -deltaV;
                    //backNavierStokesGrid.Cell(i + 1, j, k).Velocity.X += deltaV;
                    //backNavierStokesGrid.Cell(i, j, k).Velocity.Y += -deltaV;
                    //backNavierStokesGrid.Cell(i, j + 1, k).Velocity.Y += deltaV;
                    backCurr.Pressure += deltaP;

                    //curr.Velocity.X -= deltaV;
                    //nextI.Velocity.X += deltaV;
                    //curr.Velocity.Y -= deltaV;
                    //nextJ.Velocity.Y += deltaV;
                    //curr.Pressure += deltaP;
                });
                CodingHelper.Swap(ref nextNavierStokesGrid, ref backNavierStokesGrid);
            }
        }

        private void EnforceMassConservationForSurface2()
        {
            int k = 0;
            for (int j = 1; j < size.Height - 1; j++)
            for (int i = 1; i < size.Width - 1; i++)
            {
                ref var curr = ref nextNavierStokesGrid.Cell(i, j, k);

                if (curr.State != NavierStokesCellState.Liquid)
                    continue;

                ref var prevI = ref nextNavierStokesGrid.Cell(i - 1, j, k);
                ref var nextI = ref nextNavierStokesGrid.Cell(i + 1, j, k);
                ref var prevJ = ref nextNavierStokesGrid.Cell(i, j - 1, k);
                ref var nextJ = ref nextNavierStokesGrid.Cell(i, j + 1, k);

                if (prevI.State == NavierStokesCellState.Object)
                {
                    curr.Velocity.X = 0.01f;
                    prevI.Velocity.Y = curr.Velocity.Y;
                    //prevI.Pressure = curr.Pressure;
                    curr.IsSurface = true;
                }
                if (prevJ.State == NavierStokesCellState.Object)
                {
                    curr.Velocity.Y = 0.01f;
                    prevJ.Velocity.X = curr.Velocity.X;
                    //prevJ.Pressure = curr.Pressure;
                    curr.IsSurface = true;
                }
                if (nextI.State == NavierStokesCellState.Object)
                {
                    nextI.Velocity.X = -0.01f;
                    nextI.Velocity.Y = curr.Velocity.Y;
                    //nextI.Pressure = curr.Pressure;
                    curr.IsSurface = true;
                }
                if (nextJ.State == NavierStokesCellState.Object)
                {
                    nextJ.Velocity.Y = -0.01f;
                    nextJ.Velocity.X = curr.Velocity.X;
                    nextJ.Velocity.X = curr.Velocity.X;
                    //nextJ.Pressure = curr.Pressure;
                    curr.IsSurface = true;
                }

                var div = -(nextI.Velocity.X - curr.Velocity.X +
                           (nextJ.Velocity.Y - curr.Velocity.Y) +
                            0f);

                int numFreeCells = 0;
                if (prevI.State == NavierStokesCellState.Empty)
                    numFreeCells++;
                if (prevJ.State == NavierStokesCellState.Empty)
                    numFreeCells++;
                if (nextI.State == NavierStokesCellState.Empty)
                    numFreeCells++;
                if (nextJ.State == NavierStokesCellState.Empty)
                    numFreeCells++;

                if (numFreeCells > 0)
                    curr.IsSurface = true;

                var deltaDiv = div / numFreeCells;

                if (prevI.State == NavierStokesCellState.Empty)
                {
                    curr.Velocity.X += -deltaDiv;
                    prevI.Velocity.Y = curr.Velocity.Y;
                    //prevI.Pressure = atmosphericPressure;
                }
                if (prevJ.State == NavierStokesCellState.Empty)
                {
                    curr.Velocity.Y += -deltaDiv;
                    prevJ.Velocity.X = curr.Velocity.X;
                    //prevJ.Pressure = atmosphericPressure;
                }
                if (nextI.State == NavierStokesCellState.Empty)
                {
                    nextI.Velocity.X += deltaDiv;
                    nextI.Velocity.Y = curr.Velocity.Y;
                    //nextI.Pressure = atmosphericPressure;
                }
                if (nextJ.State == NavierStokesCellState.Empty)
                {
                    nextJ.Velocity.Y += deltaDiv;
                    nextJ.Velocity.X = curr.Velocity.X;
                    //nextJ.Pressure = atmosphericPressure;
                }
            }
        }

        private void MultByA(float[] v, float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = 0f;

                var cell = nextNavierStokesGrid.Cells[i];
                if (cell.State != NavierStokesCellState.Liquid || cell.IsSurface)
                    continue;

                var sum = 0f;
                int numLiquidNeighbors = 0;
                if (i >= 1)
                {
                    sum += v[i - i];
                    if (nextNavierStokesGrid.Cells[i - 1].State == NavierStokesCellState.Liquid && !nextNavierStokesGrid.Cells[i - 1].IsSurface)
                        numLiquidNeighbors++;
                }
                if (i < numCells - 1)
                {
                    sum += v[i + 1];
                    if (nextNavierStokesGrid.Cells[i + 1].State == NavierStokesCellState.Liquid && !nextNavierStokesGrid.Cells[i + 1].IsSurface)
                        numLiquidNeighbors++;
                }
                if (i >= spanJ)
                {
                    sum += v[i - spanJ];
                    if (nextNavierStokesGrid.Cells[i - spanJ].State == NavierStokesCellState.Liquid && !nextNavierStokesGrid.Cells[i - spanJ].IsSurface)
                        numLiquidNeighbors++;
                }
                if (i < numCells - spanJ)
                {
                    sum += v[i + spanJ];
                    if (nextNavierStokesGrid.Cells[i + spanJ].State == NavierStokesCellState.Liquid && !nextNavierStokesGrid.Cells[i + spanJ].IsSurface)
                        numLiquidNeighbors++;
                }
                //if (i >= spanK)
                //{
                //    sum += v[i - spanK];
                //    if (nextNavierStokesGrid.Cells[i - spanK].State == NavierStokesCellState.Liquid)
                //        numLiquidNeighbors++;
                //}
                //if (i < numCells - spanK)
                //{
                //    sum += v[i + spanK];
                //    if (nextNavierStokesGrid.Cells[i + spanK].State == NavierStokesCellState.Liquid)
                //        numLiquidNeighbors++;
                //}
                sum -= v[i] * numLiquidNeighbors;
                result[i] = sum;
            }
        }

        private static float Dot(float[] a, float[] b)
        {
            var result = 0f;
            for (int i = 0; i < a.Length; i++)
                result += a[i] * b[i];
            return result;
        }

        private void TransferMass(float deltaT)
        {
            Array.Clear(levelSet.AllMasses, 0, levelSet.AllMasses.Length);
            
            foreach (var particle in particles)
            {
                var i = (int)(particle.Position.X / levelSet.CellSize);
                var j = (int)(particle.Position.Y / levelSet.CellSize);
                var k = (int)(particle.Position.Z / levelSet.CellSize);
                levelSet.Mass(i, j) += 1f;
            }
            //levelSet.TransferMass(deltaT, nextNavierStokesGrid);
            //levelSet.RecalculatePhi();
        }

        private void MovePhi(float deltaT)
        {
            levelSet.TransferPhi(deltaT, nextNavierStokesGrid);
        }

        private void MoveParticles(float deltaT)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].Position == Vector3.Zero)
                    continue;
                var vel = nextNavierStokesGrid.GetVelocityAt(particles[i].Position);
                particles[i].Position += vel * deltaT;
                //particles[i].Position += new Vector3((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, 0) * vel.Length() / 25;
                particles[i].Position.X = MathHelper.Clamp(particles[i].Position.X, cellSize * 2 + 0.1f, (size.Width - 2) * cellSize - 0.1f);
                particles[i].Position.Y = MathHelper.Clamp(particles[i].Position.Y, cellSize * 2 + 0.1f, (size.Height - 2) * cellSize - 0.1f);
                //particles[i].Z = MathHelper.Clamp(particles[i].Z, cellSize * 2 + 0.01f, (size.Depth - 2) * cellSize - 0.01f);
            }
        }

        private void CreateAndRemoveParticles(float deltaT)
        {
            levelSet.ClearParticleMask();

            for (int p = 0; p < particles.Length; p++)
            {
                ref var particle = ref particles[p];
                if (particle.Position == Vector3.Zero)
                    continue;
                if (levelSet.PhiAt(particle.Position) < ParticleRemovalPhi)
                    RemoveParticle(p);
                else
                    levelSet.AddParticleIfHavePlace(particle.Position);
            }

            for (int j = 5; j < levelSet.Size.Height - 5; j++)
            for (int i = 5; i < levelSet.Size.Width - 5; i++)
            {
                var point = levelSet.CellSize * (new Vector3(i, j, 0) + new Vector3(0.5f, 0.5f, 0.5f));
                var phi = levelSet.Phi(i, j);
                if (phi < 0 && phi > ParticleRemovalPhi &&
                    levelSet.Curvature(point) > ParticleCreationCurvature &&
                    levelSet.AddParticleIfHavePlace(i, j))
                {
                    AddParticle(point, 1);
                }
            }
        }

        private void PostUpdateCellStates()
        {
            int k = 0;
            

            for (int j = 0; j < size.Height; j++)
            for (int i = 0; i < size.Width; i++)
            {
                ref var cell = ref nextNavierStokesGrid.Cell(i, j, k);
                cell.IsSurface = false;
                if (cell.State == NavierStokesCellState.Object)
                {
                    cell.Pressure = atmosphericPressure;
                    cell.Velocity = Vector3.Zero;
                }
                else
                {
                    cell.State = NavierStokesCellState.Empty;
                    //cell.Velocity = Vector3.Zero;
                    //cell.Pressure = atmosphericPressure;
                }
                //cell.Velocity *= 0.99f;
            }
            
            foreach (var particle in particles)
            {
                var i = (int)(particle.Position.X / cellSize);
                var j = (int)(particle.Position.Y / cellSize);
                //var k = (int)(particle.Position.Z / cellSize);
                if (nextNavierStokesGrid.Cell(i, j, k).State != NavierStokesCellState.Object)
                    nextNavierStokesGrid.Cell(i, j, k).State = NavierStokesCellState.Liquid;
            }

            for (int j = 0; j < levelSet.Size.Height; j++)
            for (int i = 0; i < levelSet.Size.Width; i++)
            {
                var nsi = i / levelSetScale;
                var nsj = j / levelSetScale;

                if (nextNavierStokesGrid.Cell(nsi, nsj, k).State == NavierStokesCellState.Empty && 
                    (levelSet.Phi(i, j) < 0 
                    /*|| levelSet.ParticleMask(i, j)*/))
                    nextNavierStokesGrid.Cell(nsi, nsj, k).State = NavierStokesCellState.Liquid;
            }

            for (int j = 0; j < size.Height; j++)
            for (int i = 0; i < size.Width; i++)
            {
                ref var cell = ref nextNavierStokesGrid.Cell(i, j, k);
                if (cell.State == NavierStokesCellState.Empty)
                {
                    cell.Pressure = atmosphericPressure;
                    cell.Velocity *= 0.90f;
                }
            }

            //foreach (var particle in particles)
            //{
            //    var i = (int)(particle.X / nextNavierStokesGrid.Size.Width);
            //    var j = (int)(particle.Y / nextNavierStokesGrid.Size.Height);
            //    var k = (int)(particle.Z / nextNavierStokesGrid.Size.Depth);
            //    if (nextNavierStokesGrid.Cell(i, j, k).State != NavierStokesCellState.Liquid)
            //        continue;
            //    if (nextNavierStokesGrid.Cell(i + 1, j, k).State == NavierStokesCellState.Empty ||
            //        nextNavierStokesGrid.Cell(i - 1, j, k).State == NavierStokesCellState.Empty ||
            //        nextNavierStokesGrid.Cell(i, j + 1, k).State == NavierStokesCellState.Empty ||
            //        nextNavierStokesGrid.Cell(i, j - 1, k).State == NavierStokesCellState.Empty ||
            //        nextNavierStokesGrid.Cell(i, j, k + 1).State == NavierStokesCellState.Empty ||
            //        nextNavierStokesGrid.Cell(i, j, k - 1).State == NavierStokesCellState.Empty)
            //    {
            //        nextNavierStokesGrid.Cell(i, j, k).State = NavierStokesCellState.Surface;
            //    }
            //}
        }

        private void RecalculateMass()
        {
            var sizeVolume = size.Volume();
            for (int i = 0; i < sizeVolume; i++)
                nextNavierStokesGrid.Cells[i].Mass = 0f;
            int k = 0;
            for (int j = 0; j < size.Height; j++)
            for (int i = 0; i < size.Width; i++)
                nextNavierStokesGrid.Cell(i, j, k).Mass += AvgLevelSetMassFor(i, j);
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].Position == Vector3.Zero)
                    continue;
                var normPos = particles[i].Position / cellSize;
                var pi = (int)normPos.X;
                var pj = (int)normPos.Y;
                var pk = (int)normPos.Z;
                nextNavierStokesGrid.Cell(pi, pj, pk).Mass += particles[i].Mass;
            }
        }

        public float AvgLevelSetMassFor(int ai, int aj)
        {
            var sum = 0f;
            for (int j = aj * levelSetScale; j < (aj + 1) * levelSetScale; j++)
            for (int i = ai * levelSetScale; i < (ai + 1) * levelSetScale; i++)
                sum += levelSet.Mass(i, j);
            return sum / (levelSetScale * levelSetScale);
        }

        public float Mass(int ai, int aj)
        {
            return nextNavierStokesGrid.Cell(ai, aj, 0).Mass;
        }

        private void AddParticle(Vector3 pos,float mass)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].Position != Vector3.Zero)
                    continue;
                particles[i].Position = pos;
                particles[i].Mass = mass;
                particles[i].TimeToLive = float.MaxValue;
                return;
            }
        }

        private void RemoveParticle(int i)
        {
            particles[i].Position = Vector3.Zero;
            particles[i].Mass = 0;
        }
    }
}