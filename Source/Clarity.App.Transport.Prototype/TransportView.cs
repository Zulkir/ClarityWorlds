using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Visualization;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.App.Transport.Prototype
{
    public abstract class TransportView : AmObjectBase<TransportView, IViewport>, IView
    {
        private readonly IRayHitIndex rayHitIndex;

        public IReadOnlyList<IViewLayer> Layers { get; }
        private readonly IControlledCamera camera;
        private readonly ViewLayer layer;

        protected TransportView(IStateVisualizer stateVisualizer, IRayHitIndex rayHitIndex)
        {
            this.rayHitIndex = rayHitIndex;
            var scene = Scene.Create(stateVisualizer.RootNode);
            scene.BackgroundColor = Color4.CornflowerBlue;
            camera = new TargetedControlledCameraY(stateVisualizer.RootNode, new TargetedControlledCameraY.Props
            {
                Target = Vector3.Zero,
                Distance = 50f,
                Pitch = MathHelper.PiOver4,
                Yaw = MathHelper.PiOver4,
                FieldOfView = MathHelper.PiOver4,
                ZNear = 0.1f,
                ZFar = 1000f
            });
            layer = new ViewLayer
            {
                VisibleScene = scene,
                Camera = camera
            };
            Layers = new IViewLayer[]
            {
                layer,
            };
        }

        public void Update(FrameTime frameTime)
        {
            camera.Update(frameTime);
        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            if (args is IMouseEventArgs margs && margs.IsLeftClickEvent())
            {
                var clickInfo = new RayHitInfo(margs.Viewport, layer, margs.State.Position);
                var hitResult = rayHitIndex.FindEntity(clickInfo);
                if (hitResult.Successful)
                {
                    foreach (var interactionComponent in hitResult.Node.SearchComponents<IInteractionComponent>())
                        if (interactionComponent.TryHandleInteractionEvent(margs))
                            return true;
                }
            }

            if (camera.TryHandleInput(args))
                return true;
            return false;
        }
    }
}