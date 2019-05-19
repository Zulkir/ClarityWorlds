using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.Core.AppCore.Views 
{
    public abstract class StoryGraphView : AmObjectBase<StoryGraphView, IViewport>, IView
    {
        private readonly IRayHitIndex rayHitIndex;
        private readonly IViewService viewService;
        private readonly ViewLayer mainLayer;
        public IReadOnlyList<IViewLayer> Layers { get; }

        protected StoryGraphView(IStoryService storyService, IRayHitIndex rayHitIndex, IViewService viewService)
        {
            this.rayHitIndex = rayHitIndex;
            this.viewService = viewService;
            var scene = storyService.EditingScene;
            mainLayer = new ViewLayer
            {
                VisibleScene = scene,
                Camera = new PlaneOrthoBoundControlledCamera(scene.Root, PlaneOrthoBoundControlledCamera.Props.Default, true)
            };
            Layers = new[] {mainLayer};
        }

        public void Update(FrameTime frameTime)
        {
            mainLayer.Camera.Update(frameTime);
        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            if (args is IMouseEventArgs margs)
            {
                if (TryHandleMouseByLayer(margs, mainLayer))
                    return true;
            }

            if (args is IKeyEventArgs kargs)
            {
                var selected = viewService.SelectedNode;
                if (selected != null)
                    foreach (var interactionComponent in selected.Node.SearchComponents<IInteractionComponent>())
                        if (interactionComponent.TryHandleInteractionEvent(kargs))
                            return true;
            }
            
            if (TryHandleInoutByCamera(args, mainLayer.Camera))
                return true;
            
            return false;
        }

        private bool TryHandleMouseByLayer(IMouseEventArgs margs, IViewLayer layer)
        {
            var clickInfo = new RayHitInfo(margs.Viewport, layer, margs.State.Position);
            var hitResult = rayHitIndex.FindEntity(clickInfo);
            if (hitResult.Successful)
            {
                foreach (var interactionComponent in hitResult.Node.SearchComponents<IInteractionComponent>())
                    if (interactionComponent.TryHandleInteractionEvent(margs))
                        return true;
            }
            else if (margs.IsClickEvent())
            {
                viewService.SelectedNode = null;
            }

            return false;
        }

        private static bool TryHandleInoutByCamera(IInputEventArgs args, ICamera camera)
        {
            if (camera is IControlledCamera controlledCamera && controlledCamera.TryHandleInput(args))
                return true;
            return false;
        }
    }
}