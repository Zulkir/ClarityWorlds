﻿using System.Collections.Generic;
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

namespace Clarity.Ext.Simulation.SpherePacking 
{
    public abstract class CirclePackingNodeComponent : SceneNodeComponentBase<CirclePackingNodeComponent>,
        ICirclePackingComponent, ITransformable3DComponent, IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly IEmbeddedResources embeddedResources;

        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public abstract float CircleRadius { get; set; }
        

        private readonly CirclePacker circlePacker;
        
        private readonly ExplicitModel borderModel;
        private CirclePackingBorder lastBorder;
        private readonly IVisualElement backgroundVisualElement;
        private readonly IVisualElement borderVisualElement;
        private readonly List<IVisualElement> circleVisualElements;
        private readonly IInteractionElement selectOnClickInterationElement;
        private readonly IRayHittable hittable;

        public float Area => circlePacker.Border.Area;
        public int MaxCircles => circlePacker.MaxNumCircles;
        public int CurrentNumCircles => circlePacker.NumCircles;

        public float RandomFactor
        {
            get => circlePacker.RandomFactor;
            set => circlePacker.RandomFactor = value;
        }

        protected CirclePackingNodeComponent(IEmbeddedResources embeddedResources, IViewService viewService, ICoroutineService coroutineService)
        {
            this.embeddedResources = embeddedResources;
            Width = 16;
            Height = 16;
            CircleRadius = 1;

            circlePacker = new CirclePacker(coroutineService);
            ResetPacker();
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
                .SetTransform(x => Transform.Translation(new Vector3(x.circlePacker.Border.BoundingRect.Center, 0)))
                .SetNonUniformScale(x => new Vector3(
                    x.circlePacker.Border.BoundingRect.HalfWidth,
                    x.circlePacker.Border.BoundingRect.HalfHeight, 
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
            hittable = new RectangleHittable<CirclePackingNodeComponent>(this, Transform.Identity, 
                x => circlePacker.Border.BoundingRect,
                x => 0);
        }

        public void ResetPacker()
        {
            circlePacker.Reset(CircleRadius, new[]
            {
                new Vector2(-5, -5),
                new Vector2(-5, 5),
                new Vector2(5, 5),
                new Vector2(5, -5),
            });
        }

        public void OptimizeStep()
        {
            circlePacker.OptimizeStep();
        }

        public void RunOptimization()
        {
            circlePacker.RunOptimization();
        }

        public Sphere LocalBoundingSphere => 
            new Sphere(Vector3.Zero, new Vector2(Width, Height).Length());

        private IModel3D GetRelevantBorderModel()
        {
            if (lastBorder == circlePacker.Border)
                return borderModel;
            lastBorder = circlePacker.Border;
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
            for (int i = 0; i < circlePacker.NumCircles; i++)
            {
                var iLoc = i;
                if (circleVisualElements.Count <= i)
                    circleVisualElements.Add(ModelVisualElement.New(this)
                        .SetModel(x => x.embeddedResources.CircleModel(64))
                        .SetMaterial(StandardMaterial.New(this)
                            .SetIgnoreLighting(true)
                            .SetDiffuseColor(Color4.Yellow))
                        .SetTransform(x => new Transform(x.circlePacker.CircleRadius, Quaternion.Identity, new Vector3(x.circlePacker.FrontCircleCenters[iLoc], 0))));
                yield return circleVisualElements[i];
            }
        }

        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return selectOnClickInterationElement.TryHandleInteractionEvent(args);
        }

        public RayHitResult HitWithClick(RayCastInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}