using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.External.SpherePacking;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.Interaction.Manipulation3D;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Media.Models.Explicit;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public abstract class CirclePackingAutoComponent : SceneNodeComponentBase<CirclePackingAutoComponent>,
        ICirclePackingAutoComponent, ITransformable3DComponent, IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly IEmbeddedResources embeddedResources;
        private readonly ICoroutineService coroutineService;

        public string ShapeName { get; set; } = "Square";
        public float CircleRadius { get; set; } = 1;
        public float Precision { get; set; } = 1e-3f;
        public int MaxIterationsPerAttempt { get; set; } = 10000;
        public int CostDecreaseGracePeriod { get; set; } = 1000;
        public int ShakeIterations { get; set; } = 100;
        public float ShakeStrength { get; set; } = 0.5f;
        public float MinCostDecrease { get; set; } = 0;
        public int AttemptsPerRefresh { get; set; } = 1;

        private readonly CirclePackingSolver solver;
        private CirclePackingSolvingProcess solvingProcess;
        private ICirclePackingBorder border;
        private bool runAsync;

        private readonly ExplicitModel borderModel;
        private ICirclePackingBorder lastBorder;
        private readonly IVisualElement backgroundVisualElement;
        private readonly IVisualElement borderVisualElement;
        private readonly List<IVisualElement> circleVisualElements;
        private readonly IInteractionElement selectOnClickInterationElement;
        private readonly IRayHittable hittable;

        public int NumCircles => solvingProcess.Packer.NumCircles;
        public int AttemptsSinceLastSuccess => solvingProcess.Status.AttemptsSinceLastSuccess;
        public float SecondsSinceLastSuccess => solvingProcess.Status.SecondsSinceLastSuccess;

        protected CirclePackingAutoComponent(IEmbeddedResources embeddedResources, IViewService viewService, ICoroutineService coroutineService)
        {
            this.embeddedResources = embeddedResources;
            this.coroutineService = coroutineService;

            solver = new CirclePackingSolver();
            Reset();

            borderModel = new ExplicitModel(ResourceVolatility.Stable)
            {
                IndexSubranges = new ExplicitModelIndexSubrange[1],
                Topology = ExplicitModelPrimitiveTopology.LineStrip
            };
            backgroundVisualElement = ModelVisualElement.New(this)
                .SetModel(embeddedResources.SimplePlaneXyModel())
                .SetMaterial(StandardMaterial.New()
                    .SetIgnoreLighting(true)
                    .SetDiffuseColor(Color4.Black)
                    .FromGlobalCache())
                .SetRenderState(StandardRenderState.New()
                    .SetZOffset(-GraphicsHelper.MinZOffset))
                .SetTransform(x => Transform.Translation(new Vector3(x.border.BoundingRect.Center, 0)))
                .SetNonUniformScale(x => new Vector3(
                    x.border.BoundingRect.HalfWidth,
                    x.border.BoundingRect.HalfHeight,
                    1));
            borderVisualElement = ModelVisualElement.New(this)
                .SetModel(x => x.GetRelevantBorderModel())
                .SetMaterial(StandardMaterial.New()
                    .SetDiffuseColor(Color4.Yellow)
                    .SetIgnoreLighting(true)
                    .FromGlobalCache());
            circleVisualElements = new List<IVisualElement>();
            selectOnClickInterationElement = new SelectOnClickInteractionElement(this, viewService);
            // todo: make precise
            hittable = new RectangleHittable<CirclePackingAutoComponent>(this, Transform.Identity,
                x => x.border.BoundingRect,
                x => 0);
        }

        public void Reset()
        {
            border = BorderByName(ShapeName);
            solvingProcess = solver.Solve(new CirclePackingSolverSettings
            {
                CircleRadius = CircleRadius,
                Precision = Precision,
                Border = border,
                MaxIterationsPerAttempt = MaxIterationsPerAttempt,
                CostDecreaseGracePeriod = CostDecreaseGracePeriod,
                ShakeIterations = ShakeIterations,
                ShakeStrength = ShakeStrength,
                MinCostDecrease = MinCostDecrease
            });
        }

        private ICirclePackingBorder BorderByName(string borderName)
        {
            Vector2[] borderPoints;
            switch (borderName)
            {
                case "Square":
                    borderPoints = new[]
                    {
                        new Vector2(-5, -5),
                        new Vector2(-5, 5),
                        new Vector2(5, 5),
                        new Vector2(5, -5),
                    };
                    break;
                case "Circle":
                    borderPoints = Enumerable.Range(0, 256)
                        .Select(x => x * MathHelper.TwoPi / 256)
                        .Select(x => 5 * new Vector2(MathHelper.Cos(x), MathHelper.Sin(x)))
                        .ToArray();
                    break;
                case "Ellipse":
                    borderPoints = Enumerable.Range(0, 256)
                        .Select(x => x * MathHelper.TwoPi / 256)
                        .Select(x => new Vector2(8 * MathHelper.Cos(x), 5 * MathHelper.Sin(x)))
                        .ToArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new PolylineCirclePackingBorder(borderPoints, CircleRadius);
        }

        public void Run()
        {
            RunAsync();
        }

        private async void RunAsync()
        {
            runAsync = true;
            while (runAsync)
            {
                solvingProcess.RunFor(AttemptsPerRefresh);
                await coroutineService.WaitUpdates(1);
            }
        }

        public void Stop()
        {
            runAsync = false;
        }

        public Sphere LocalBoundingSphere
        {
            get
            {
                var boundingCircle = border.BoundingRect.GetBoundingCircle();
                return new Sphere(new Vector3(boundingCircle.Center, 0), boundingCircle.Radius);
            }
        }

        private IModel3D GetRelevantBorderModel()
        {
            if (lastBorder == border)
                return borderModel;
            lastBorder = border;
            borderModel.Positions = lastBorder.Points.ConcatSingle(lastBorder.Points[0]).Select(x => new Vector3(x, 0)).ToArray();
            borderModel.Indices = Enumerable.Range(0, lastBorder.Points.Count + 1).ToArray();
            borderModel.IndexSubranges[0] = new ExplicitModelIndexSubrange(0, borderModel.Indices.Length);
            borderModel.RecalculateInfo();
            borderModel.OnModified(null);
            return borderModel;
        }

        public IEnumerable<IVisualElement> GetVisualElements()
        {
            yield return borderVisualElement;
            yield return backgroundVisualElement;
            for (int i = 0; i < solvingProcess.Packer.NumCircles; i++)
            {
                var iLoc = i;
                if (circleVisualElements.Count <= i)
                    circleVisualElements.Add(ModelVisualElement.New(this)
                        .SetModel(x => x.embeddedResources.CircleModel(64))
                        .SetMaterial(StandardMaterial.New(this)
                            .SetIgnoreLighting(true)
                            .SetDiffuseColor(x => x.ColorForStatus(x.solvingProcess.Packer.FrontCircleStatuses[iLoc])))
                        .SetTransform(x => new Transform(x.solvingProcess.Packer.CircleRadius, Quaternion.Identity, new Vector3(x.solvingProcess.Packer.FrontCircleCenters[iLoc], 0))));
                yield return circleVisualElements[i];
            }
        }

        private Color4 ColorForStatus(CircleStatus status)
        {
            if (status.MinDistance >= 2 * CircleRadius - Precision)
                return Color4.Green;
            var relativeDistance = status.MinDistance / CircleRadius;
            return Color4.Lerp(Color4.Red, Color4.Yellow, MathHelper.Clamp(MathHelper.Pow(relativeDistance - 1, 32), 0, 1));
        }

        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return selectOnClickInterationElement.TryHandleInteractionEvent(args);
        }

        public RayHitResult HitWithClick(RayCastInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}