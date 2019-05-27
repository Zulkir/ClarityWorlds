using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Visualization.Views
{
    public interface IView : IAmObject<IViewport>
    {
        IReadOnlyList<IViewLayer> Layers { get; }
        void Update(FrameTime frameTime);
        bool TryHandleInput(IInputEventArgs args);
        void OnEveryEvent(IRoutedEvent evnt);
    }
}