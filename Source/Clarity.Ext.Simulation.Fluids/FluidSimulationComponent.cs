using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.Interaction;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Core.External.FluidSimulation;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Ext.Simulation.Fluids
{
    public abstract class FluidSimulationComponent : SceneNodeComponentBase<FluidSimulationComponent>, IFluidSimulationComponent, 
        ITransformable3DComponent, IVisualComponent, IInteractionComponent, IRayHittable
    {
        private readonly IEmbeddedResources embeddedResources;
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public abstract float CellSize { get; set; }
        public abstract int LevelSetScale { get; set; }
        public abstract FluidSurfaceType SurfaceType { get; set; }

        private readonly List<IVisualElement> visualElements;
        private readonly IInteractionElement[] interactionElems;
        private readonly IRayHittable hittable;
        private readonly IFluidSimulation fluidSimulation;
        private RawImage levelSetImage;
        private byte[] levelSetImageData;
        private IRawDataResource particlesBuffer;
        private IFlexibleModel model;
        private IFlexibleModel squareModel;
        private bool simulationRunning;
        private float simulationTimestamp;
        private Queue<FluidSimulationFrame> prevQueue;
        private FluidSimulationFrame prevFrame;
        private FluidSimulationFrame nextFrame;
        private bool firstTime = true;

        public float OwnRadius => model.Radius;

        protected FluidSimulationComponent(IEmbeddedResources embeddedResources) 
        {
            this.embeddedResources = embeddedResources;
            //var size = new IntSize3(20, 20, 1);
            //var cellSize = 0.40f;
            //var particleRadius = 0.25f;
            fluidSimulation = new FluidSimulation(1f, 1f, 0.25f);

            visualElements = new List<IVisualElement>();
            interactionElems = new IInteractionElement[]
            {
                //new SelectOnClickInteractionElement(this, viewService),
                new ActionOnEventInteractionElement(
                    args => args is IMouseEventArgs m && m.IsRightClickEvent() && m.KeyModifyers == KeyModifyers.None,
                    () =>
                    {
                        if (simulationRunning)
                        {
                            fluidSimulation.Stop();
                        }
                        else
                        {
                            if (firstTime)
                            {
                                fluidSimulation.Reset(CreateConfig());
                                Reset();
                                firstTime = false;
                            }
                            prevFrame = nextFrame = null;
                            fluidSimulation.Run(simulationTimestamp + 5);
                        }
                        simulationRunning = !simulationRunning;
                    }),
                new ActionOnEventInteractionElement(
                    args => args is IMouseEventArgs m && m.IsRightClickEvent() && m.KeyModifyers == KeyModifyers.Shift,
                    () =>
                    {
                        Reset();
                    }),
            };

            hittable = new SphereHittable<FluidSimulationComponent>(this, c =>
            {
                var globalTransform1 = c.Node.GlobalTransform;
                return new Sphere(globalTransform1.Offset, c.model.Radius * globalTransform1.Scale);
            });

            Width = 20;
            Height = 20;
            CellSize = 0.8f;
            LevelSetScale = 16;
            SurfaceType = FluidSurfaceType.Hybrid;
            Reset();
        }

        private void Reset()
        {
            simulationRunning = false;
            simulationTimestamp = 0;
            prevQueue = new Queue<FluidSimulationFrame>();
            var size = new IntSize3(Width, Height, 1);
            var cellSize = CellSize;
            fluidSimulation.Reset(CreateConfig());
            model = CreateModel(size, cellSize, fluidSimulation.Particles.Length);
            levelSetImageData = new byte[fluidSimulation.LevelSet.Size.Width * fluidSimulation.LevelSet.Size.Height * 4];
            levelSetImage = new RawImage(ResourceVolatility.Volatile, new IntSize2(fluidSimulation.LevelSet.Size.Width, fluidSimulation.LevelSet.Size.Height), false, levelSetImageData);
            squareModel = embeddedResources.SimplePlaneXyModel();
            visualElements.Clear();
            visualElements.Add(new CgModelVisualElement()
                .SetModel(model)
                .SetMaterial(new StandardMaterial(new SingleColorPixelSource(Color4.Yellow)) { IgnoreLighting = true }));
            visualElements.Add(new CgModelVisualElement()
                .SetModel(model)
                .SetModelPartIndex(0)
                .SetMaterial(new StandardMaterial(new SingleColorPixelSource(Color4.White)) { IgnoreLighting = true }));
            visualElements.Add(new CgModelVisualElement()
                .SetModel(squareModel)
                .SetMaterial(new StandardMaterial(levelSetImage) {IgnoreLighting = true})
                .SetTransform(new Transform(cellSize * size.Width / 2, Quaternion.Identity, new Vector3(cellSize * size.Width / 2, cellSize * size.Height / 2, -0.1f))));
        }

        private FluidSimulationConfig CreateConfig()
        {
            return new FluidSimulationConfig
            {
                Width = Width,
                Height = Height,
                CellSize = CellSize,
                LevelSetScale = LevelSetScale,
                SurfaceType = SurfaceType
            };
        }

        //public override void Setup(WorldNodeSetupContext setupContext)
        //{
        //    setupContext.GetOrAddAspect<IRayHittableComponent>(() => new HittableAspect(Node))
        //        .Setup.Override(new SphereHittable<FluidSimulationComponent>(this, c =>
        //        {
        //            var globalTransform1 = c.Node.GlobalTransform;
        //            return new Sphere(globalTransform1.Offset, c.model.Radius * globalTransform1.Scale);
        //        }));
        //}

        public override void Update(FrameTime frameTime)
        {
            if (simulationRunning)
            {
                if (TryMoveToCorrectFrames())
                {
                    simulationTimestamp += frameTime.DeltaSeconds;
                    if (prevFrame != null && nextFrame != null)
                    {
                        var lerpAmount = (simulationTimestamp - prevFrame.Timestamp) / (nextFrame.Timestamp - prevFrame.Timestamp);
                        UpdateModel(lerpAmount);
                        UpdateVisuals(lerpAmount);
                    }
                }
            }
            base.Update(frameTime);
        }

        

        private bool TryMoveToCorrectFrames()
        {
            while (nextFrame == null || simulationTimestamp > nextFrame.Timestamp)
            {
                if (!fluidSimulation.FrameQueue.TryDequeue(out var frame))
                    return false;
                nextFrame = frame;
                prevQueue.Enqueue(frame);
            }
            while (prevQueue.Peek().Timestamp < simulationTimestamp)
            {
                prevFrame = prevQueue.Dequeue();
            }
            return true;
        }

        private unsafe IFlexibleModel CreateModel(IntSize3 size, float cellSize, int numParticles)
        {
            // todo: add a 'IResourceSource.AttachResource(resource)' method
            var vertexPosElemInfo = new [] { new VertexElementInfo(CommonVertexSemantic.Position, 0, CommonFormat.R32G32B32_SFLOAT, 0, sizeof(Vector3)) };
            var resourcePack = new ResourcePack(ResourceVolatility.Immutable);

            // Bounds
            var bounds = new Vector3(size.Width * cellSize, size.Height * cellSize, size.Depth * cellSize);
            var boundsVertices = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, bounds.Y, 0),
                new Vector3(bounds.X, bounds.Y, 0),
                new Vector3(bounds.X, 0, 0),
                new Vector3(0, 0, bounds.Z),
                new Vector3(0, bounds.Y, bounds.Z),
                new Vector3(bounds.X, bounds.Y, bounds.Z),
                new Vector3(bounds.X, 0, bounds.Z),
            };
            var boundsIndices = new ushort[]
            {
                0, 1, 1, 2, 2, 3, 3, 0,
                4, 5, 5, 6, 6, 7, 7, 4,
                0, 4, 1, 5, 2, 6, 3, 7
            };
            RawDataResource boundsVertexBuffer;
            fixed (Vector3* pBoundsVertexData = boundsVertices)
                boundsVertexBuffer = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pBoundsVertexData, boundsVertices.Length * sizeof(Vector3));
            resourcePack.AddSubresource("BoundsVertexBuffer", boundsVertexBuffer);
            RawDataResource boundsIndexBuffer;
            fixed (ushort* pBoundsIndices = boundsIndices)
                boundsIndexBuffer = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pBoundsIndices, boundsIndices.Length * sizeof(ushort));
            resourcePack.AddSubresource("BoundsIndexBuffer", boundsIndexBuffer);
            var boundsVertexSet = new FlexibleModelVertexSet(ResourceVolatility.Immutable, 
                new [] { boundsVertexBuffer.AsSubrange(), boundsIndexBuffer.AsSubrange() }, 
                vertexPosElemInfo, 
                new VertexIndicesInfo(1, CommonFormat.R16_UINT));
            resourcePack.AddSubresource("BoundsVertexSet", boundsVertexSet);
            var boundsModelPart = new FlexibleModelPart
            {
                VertexSetIndex = 0,
                PrimitiveTopology = FlexibleModelPrimitiveTopology.LineList,
                ModelMaterialName = "BoundsMaterial",
                IndexCount = boundsIndices.Length
            };
            
            // Particles
            particlesBuffer = new RawDataResource(ResourceVolatility.Volatile, numParticles * sizeof(Vector3));
            var pParticlePositions = (Vector3*)particlesBuffer.Map();
            for (int i = 0; i < fluidSimulation.Particles.Length; i++)
                pParticlePositions[i] = fluidSimulation.Particles[i].Position;
            particlesBuffer.Unmap(true);
            resourcePack.AddSubresource("ParticlesBuffer", particlesBuffer);

            var particlesVertexSet = new FlexibleModelVertexSet(ResourceVolatility.Immutable,
                new [] { particlesBuffer.AsSubrange() },
                vertexPosElemInfo,
                null);
            resourcePack.AddSubresource("ParticlesVertexSet", particlesVertexSet);

            var particlesModelPart = new FlexibleModelPart
            {
                VertexSetIndex = 1,
                PrimitiveTopology = FlexibleModelPrimitiveTopology.PointList,
                ModelMaterialName = "ParticlesMaterial",
                IndexCount = numParticles
            };

            var resultModel = new FlexibleModel(ResourceVolatility.Immutable, 
                new [] {boundsVertexSet, particlesVertexSet}, 
                new [] { boundsModelPart, particlesModelPart }, 
                bounds.Length());
            resourcePack.AddSubresource("Model", resultModel);
            return resultModel;
        }

        private unsafe void UpdateVisuals(float lerpAmount)
        {
            var cellSize = fluidSimulation.CurrentNavierStokesGrid.CellSize;
            var size = fluidSimulation.CurrentNavierStokesGrid.Size;
            visualElements.RemoveRange(3, visualElements.Count - 3);

            const int blue = 0;
            const int green = 1;
            const int red = 2;
            const int alpha = 3;

            var ii = 0;
            fixed (byte* pLevelSetImageData = levelSetImageData)
            {
                var pixels = (int*)pLevelSetImageData;
                for (int j = fluidSimulation.LevelSet.Size.Height - 1; j >= 0; j--)
                for (int i = 0; i < fluidSimulation.LevelSet.Size.Width; i++)
                {
                    var index = j * prevFrame.LeveSetSizeStokesSize.Width + i;
                    var phi = nextFrame.Phi[index];
                    var particleMask = MathHelper.Lerp(prevFrame.ParticleMask[index] ? 1 : 0, nextFrame.ParticleMask[index] ? 1 : 0, lerpAmount);
                    var isLiquid = particleMask > 0;
                    if (ii * 4 >= levelSetImageData.Length)
                        throw new Exception();
                    var channels = (byte*)&pixels[ii];

                    channels[blue] = (byte)((isLiquid || phi < 0) ? 127 : 0);
                    channels[green] = (byte)(false && phi < 0 ? 127 : 0);
                    channels[red] = (byte)(0);
                    //channels[red] = (byte)(particleMask * 127);
                    channels[alpha] = 0;
                    ii++;
                }
            }
            levelSetImage.OnModified(null);
        }

        private unsafe void UpdateModel(float lerpAmount)
        {

            var pParticlePositions = (Vector3*)particlesBuffer.Map();
            for (int i = 0; i < prevFrame.Particles.Length; i++)
            {
                var prevParticle = prevFrame.Particles[i];
                var nextParticle = nextFrame.Particles[i];
                if ((prevParticle - nextParticle).LengthSquared() < 1 && prevParticle != Vector3.Zero && nextParticle != Vector3.Zero)
                    pParticlePositions[i] = Vector3.Lerp(prevParticle, nextParticle, lerpAmount);
                else
                    pParticlePositions[i] = nextParticle;
            }
            particlesBuffer.Unmap(true);
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements() => visualElements;

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            return interactionElems.Any(elem => elem.TryHandleInteractionEvent(args));
        }

        // Hittable
        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}