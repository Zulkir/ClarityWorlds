using System.Collections.Generic;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Gui;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Platforms
{
    public interface IRenderService
    {
        IImage CreateRenderTargetImage(IntSize2 size);
        void Render(IRenderGuiControl target, float timestamp);
        void Render(IImage target, IReadOnlyList<IViewport> viewports, float timestamp);
    }
}