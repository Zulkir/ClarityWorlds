using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryGraph.Editing.Flowchart;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.App.Worlds.Views 
{
    public abstract class StoryGraphView : AmObjectBase<StoryGraphView, IViewport>, IView
    {
        private readonly IStoryService storyService;

        private readonly ViewLayer mainLayer;
        private readonly IScene scene;
        private readonly PlaneOrthoBoundControlledCamera camera;
        public IReadOnlyList<IViewLayer> Layers { get; }

        protected StoryGraphView(IStoryService storyService)
        {
            this.storyService = storyService;
            scene = storyService.EditingScene;
            GetCameraProps(scene, out var cameraProps, out var cameraBounds);
            camera = new PlaneOrthoBoundControlledCamera(scene.Root, cameraProps, true, cameraBounds);
            mainLayer = new ViewLayer
            {
                VisibleScene = scene,
                Camera = camera
            };
            Layers = new[] {mainLayer};
        }

        public void Update(FrameTime frameTime)
        {
            mainLayer.Camera.Update(frameTime);
        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            return false;
        }

        public void OnEveryEvent(IRoutedEvent evnt)
        {
            if (evnt is IWorldTreeUpdatedEvent worldChangedEvent)
            {
                if (worldChangedEvent.AmMessage.Obj<WorldHolder>().ValueChanged(x => x.World, out _))
                {
                    GetCameraProps(scene, out var props, out var bounds);
                    camera.SetProperties(props);
                    camera.SetBounds(bounds);
                }
                else if (worldChangedEvent.AmMessage.Obj<ISceneNode>().ItemAddedOrRemoved(x => x.ChildNodes, out _))
                {
                    GetCameraProps(scene, out _, out var bounds);
                    camera.SetBounds(bounds);
                }
            }
        }

        private static void GetCameraProps(IScene editingScene, 
            out PlaneOrthoBoundControlledCamera.Props props, 
            out PlaneOrthoBoundControlledCamera.CameraBounds bounds)
        {
            // todo: use StoryFlowchartEditSceneComponent.DefaultViewpointMechanism
            props = PlaneOrthoBoundControlledCamera.Props.Default;
            bounds.PlaneBounds = editingScene.Root.ChildNodes.First().GetComponent<StoryFlowchartNodeGizmoComponent>().GlobalRectangle;
            bounds.MaxDistance = Math.Max(bounds.PlaneBounds.Width, bounds.PlaneBounds.Height) / 2 / MathHelper.Tan(MathHelper.Pi / 8);
            bounds.MinDistance = 2;
            props.Distance = bounds.MaxDistance;
        }
    }
}