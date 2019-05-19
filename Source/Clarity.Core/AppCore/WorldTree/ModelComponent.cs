using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.AppModes;
using Clarity.Core.AppCore.Interaction;
using Clarity.Core.AppCore.Views;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppCore.WorldTree
{
    public abstract class ModelComponent : SceneNodeComponentBase<ModelComponent>, IModelComponent, 
        IVisualComponent, ITransformable3DComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly Lazy<IAppModeService> appModeServiceLazy;

        public abstract IFlexibleModel Model { get; set; }
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

        public float OwnRadius => Model.Radius;

        protected ModelComponent(IViewService viewService, Lazy<IAppModeService> appModeServiceLazy)
        {
            this.appModeServiceLazy = appModeServiceLazy;
            SingleColor = true;
            selectInteractionELement = new SelectOnClickInteractionElement(this, viewService);
            hittable = new SphereHittable<IModelComponent>(this, c =>
            {
                var globalTransform1 = c.Node.GlobalTransform;
                return new Sphere(globalTransform1.Offset, c.Model.Radius * globalTransform1.Scale);
            });
        }

        private void BuildVisualElements()
        {
            if (Model == null)
            {
                visualElems = EmptyArrays<IVisualElement>.Array;
                return;
            }

            visualElems = new IVisualElement[Model.Parts.Count];
            for (var i = 0; i < Model.Parts.Count; i++)
            {
                var iLoc = i;
                var material = new StandardMaterialProxy<ModelComponent>(this)
                {
                    GetDiffuseTextureSource = x => x.Texture != null
                        ? (IPixelSource)x.Texture
                        : (IPixelSource)new SingleColorPixelSource(x.GetColor(iLoc)),
                    GetNoSpecular = x => x.NoSpecular,
                    GetIgnoreLighting = x => x.IgnoreLighting,
                    //GetLineWidth = x => 1,
                    //GetPointSize = x => 1,
                };

                var elem = new CgModelVisualElement<ModelComponent>(this)
                    .SetModel(Model)
                    .SetModelPartIndex(i)
                    .SetMaterial(material)
                    .SetCullFace(x => x.DontCull ? CgCullFace.None : CgCullFace.Back)
                    .SetTransformSpace(x => x.Ortho ? CgTransformSpace.Ortho : CgTransformSpace.Scene);
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

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            return appModeServiceLazy.Value.Mode == AppMode.Editing &&
                   selectInteractionELement.TryHandleInteractionEvent(args);
        }

        // Hittable
        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}