using System.Collections.Generic;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Visualization.Views
{
    public interface IView
    {
        IReadOnlyList<IViewLayer> Layers { get; }
        void Update(FrameTime frameTime);
        bool TryHandleInput(IInputEvent args);
        void OnEveryEvent(IRoutedEvent evnt);
    }
}