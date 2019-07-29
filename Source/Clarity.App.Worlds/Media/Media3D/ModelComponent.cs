using System;
using System.Collections.Generic;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.Interaction.Manipulation3D;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.App.Worlds.Media.Media3D
{
    public abstract class ModelComponent : SceneNodeComponentBase<ModelComponent>, IModelComponent, 
        IVisualComponent, ITransformable3DComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly Lazy<IAppModeService> appModeServiceLazy;

        public abstract IModel3D Model { get; set; }
        public abstract Color4 Color { get; set; }
        public abstract bool IgnoreLighting { get; set; }
        public abstract bool NoSpecular { get; set; }
        public abstract IImage Texture { get; set; }
        public abstract bool SingleColor { get; set; }
        public abstract bool Ortho { get; set; }
        public abstract bool DontCull { get; set; }
        
        private IVisualElement[] visualElems;
        private readonly IInteractionElement selectInteractionELement;
        private readonly IRayHittable hittable;

        public Sphere LocalBoundingSphere => Model.BoundingSphere;

        protected ModelComponent(IViewService viewService, Lazy<IAppModeService> appModeServiceLazy)
        {
            this.appModeServiceLazy = appModeServiceLazy;
            SingleColor = true;
            selectInteractionELement = new SelectOnClickInteractionElement(this, viewService);
            hittable = new SphereHittable<IModelComponent>(this, c => c.Model.BoundingSphere * c.Node.GlobalTransform);
        }

        private void BuildVisualElements()
        {
            if (Model == null)
            {
                visualElems = EmptyArrays<IVisualElement>.Array;
                return;
            }

            visualElems = new IVisualElement[Model.PartCount];
            for (var i = 0; i < Model.PartCount; i++)
            {
                var iLoc = i;
                var material = StandardMaterial.New(this)
                    .SetDiffuseColor(x => x.GetColor(iLoc))
                    .SetDiffuseMap(x => x.Texture)
                    .SetNoSpecular(x => x.NoSpecular)
                    .SetIgnoreLighting(x => x.IgnoreLighting);

                var elem = new ModelVisualElement<ModelComponent>(this)
                    .SetModel(Model)
                    .SetModelPartIndex(i)
                    .SetMaterial(material)
                    .SetRenderState(StandardRenderState.New(this)
                        .SetCullFace(x => x.DontCull ? CullFace.None : CullFace.Back))
                    .SetTransformSpace(x => x.Ortho ? TransformSpace.Ortho : TransformSpace.Scene);
                visualElems[i] = elem;
            }
        }

        private Color4 GetColor(int index)
        {
            if (SingleColor)
                return Color;

            index %= 7;
            switch (index)
            {
                case 0: return Color4.Red;
                case 1: return Color4.Green;
                case 2: return Color4.Blue;
                case 3: return Color4.Yellow;
                case 4: return Color4.Magenta;
                case 5: return Color4.Cyan;
                case 6: return Color4.White;
                default: return Color4.CornflowerBlue;
            }
        }

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            //if (AmParent == null)
            //    return;
            if (message.Obj(this).ValueChanged(x => x.Model, out _))
                BuildVisualElements();
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements() => visualElems;
        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return appModeServiceLazy.Value.Mode == AppMode.Editing &&
                   selectInteractionELement.TryHandleInteractionEvent(args);
        }

        // Hittable
        public RayHitResult HitWithClick(RayCastInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}