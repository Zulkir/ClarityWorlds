using System.Collections.Generic;
using Clarity.Engine.Gui;
using Clarity.Engine.Visualization.Viewports;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IRenderingRuntime
    {
        void RenderToContext(IRenderGuiControl renderControl, float timestamp);
        void RenderToTexture(ITexture2D glTexture, IReadOnlyList<IViewport> viewports, float timestamp);
    }
}