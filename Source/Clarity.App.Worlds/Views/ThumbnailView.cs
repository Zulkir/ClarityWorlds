using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.App.Worlds.Views
{
    public abstract class ThumbnailView : AmObjectBase<ThumbnailView, IViewport>, IView
    {
        public IReadOnlyList<IViewLayer> Layers { get; private set; }
        
        public void Init(IFocusNodeComponent cFocus)
        {
            Layers = new[] {new ViewLayer
            {
                VisibleScene = cFocus.Node.Scene,
                Camera = cFocus.DefaultViewpointMechanism.FixedCamera
            }};
        }

        public void Update(FrameTime frameTime)
        {
        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            return false;
        }

        public static ThumbnailView Create(IFocusNodeComponent cFocus)
        {
            var view = AmFactory.Create<ThumbnailView>();
            view.Init(cFocus);
            return view;
        }

        public void OnEveryEvent(IRoutedEvent evnt)
        {
        }
    }
}