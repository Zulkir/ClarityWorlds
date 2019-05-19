﻿using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppCore.Interaction.Queries.Scene
{
    public abstract class UserQuerySceneComponent : SceneNodeComponentBase<UserQuerySceneComponent>, IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly IUserQueryService queryService;
        private readonly IFlexibleModel planeModel;

        private readonly List<AaRectangle2> optionRects;
        private readonly List<IVisualElement> visualElements;
        private readonly IRayHittable hittable;
        private OptionsUserQuery currentQuery;

        protected UserQuerySceneComponent(IUserQueryService queryService, IEmbeddedResources embeddedResources)
        {
            this.queryService = queryService;
            planeModel = embeddedResources.SimplePlaneXyModel();

            optionRects = new List<AaRectangle2>();
            visualElements = new List<IVisualElement>();
            hittable = new RectangleHittable<UserQuerySceneComponent>(this,
                       Transform.Identity, x => new AaRectangle2(Vector2.Zero, 1f, 1f), x => 0);
        }

        public void OnQueryServiceUpdated()
        {
            visualElements.Clear();
            optionRects.Clear();

            currentQuery = queryService.Queries.LastOrDefault() as OptionsUserQuery;
            if (currentQuery == null || !currentQuery.Options.Any()) 
                return;
            const float OptionMargin = 0.1f;
            const float OptionHalfHeight = 0.07f;
            const float OptionHalfWidth = 0.9f;
            var currentY = 1f;
            for (var i = 0; i < currentQuery.Options.Count; i++)
            {
                currentY -= OptionMargin;
                currentY -= OptionHalfHeight;
                optionRects.Add(new AaRectangle2(new Vector2(0, currentY), OptionHalfWidth, OptionHalfHeight));
                currentY -= OptionHalfHeight;
                currentY -= OptionMargin;
            }
            var totalHeight = 1f - currentY;
            var centerY = 1f - totalHeight / 2;
            for (int i = 0; i < currentQuery.Options.Count; i++)
            {
                var option = currentQuery.Options[i];

                var rect = optionRects[i];
                rect.Center.Y -= centerY;
                optionRects[i] = rect;

                var textBox = RichTextHelper.Label(option,
                    new IntSize2((int)(2 * 512 * OptionHalfWidth), (int)(2 * 512 * OptionHalfHeight)),
                    RtParagraphAlignment.Center,
                    Color4.Black,
                    RtTransparencyMode.Opaque,
                    "Arial",
                    36,
                    Color4.Orange,
                    FontDecoration.None);
                var material = new StandardMaterial(new RichTextPixelSource(textBox))
                {
                    IgnoreLighting = true
                };

                visualElements.Add(new CgModelVisualElement()
                    .SetModel(planeModel)
                    .SetMaterial(material)
                    .SetTransform(Transform.Translation(new Vector3(rect.Center, 0)))
                    .SetNonUniformScale(new Vector3(rect.HalfWidth, rect.HalfHeight, 1)));
            }
        }

        public IEnumerable<IVisualElement> GetVisualElements()
        {
            return visualElements;
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (currentQuery == null)
                return false;
            switch (args)
            {
                case IMouseEventArgs margs:
                    // todo: calc point correctly (from the hittable to the click index)
                    if (!margs.IsLeftClickEvent())
                        break;
                    var point = margs.State.NormalizedPosition;
                    var hitRect = optionRects.SelectWithIndex().Where(x => x.Value.ContainsPoint(point)).FirstOrNull();
                    if (!hitRect.HasValue)
                        break;
                    var option = hitRect.Value.Key;
                    currentQuery.Choose(option);
                    break;
                case IKeyEventArgs kargs:
                    break;
            }
            return true;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo)
        {
            return hittable.HitWithClick(clickInfo);
        }
    }
}