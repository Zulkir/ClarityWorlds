using System.Collections.Generic;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.App.Worlds.Views 
{
    public abstract class StoryGraphView : AmObjectBase<StoryGraphView, IViewport>, IView
    {
        private readonly ViewLayer mainLayer;
        public IReadOnlyList<IViewLayer> Layers { get; }

        protected StoryGraphView(IStoryService storyService)
        {
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
            return false;
        }

        public void OnEveryEvent(IRoutedEvent evnt)
        {
            
        }
    }
}