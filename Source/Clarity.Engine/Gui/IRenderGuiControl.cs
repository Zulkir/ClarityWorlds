using System.Collections.Generic;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Gui
{
    // todo: to IRenderAreaControl
    public interface IRenderGuiControl
    {
        object RenderLibHandle { get; }
        int Width { get; }
        int Height { get; }

        IReadOnlyList<IViewport> Viewports { get; }
        void SetViewports(IReadOnlyList<IViewport> viewports, ViewportsLayout layout);
    }
}