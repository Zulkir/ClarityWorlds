using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Exceptions;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Gui;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Ext.Rendering.Ogl3.Implementations;
using ObjectGL.Api.Context;
using ObjectGL.CachingImpl.Context;
using ObjectGL.Otk;
using OpenTK.Graphics;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class RenderService : IRenderService
    {
        private readonly IGraphicsInfra infra;
        private readonly IRenderingRuntime renderingRuntime;

        public RenderService(IDiContainer di, IWindowingSystem windowingSystem, IRenderLoopDispatcher renderLoopDispatcher)
        {
            var renderingArea = windowingSystem.RenderControl;

            if (renderingArea.RenderLibHandle == null)
                throw new InvalidOperationException(string.Format("Trying to initialize a rendering system while {0} is null",
                    "renderControl.RenderLibHandle"));
            if (!(renderingArea.RenderLibHandle is IGraphicsContext))
                throw new NotSupportedException(string.Format("RenderingSystemOgl requires {0} to implement interface {1}",
                    "renderControl.RenderLibHandle", typeof(IGraphicsContext).FullName));

            var tkContext = (IGraphicsContext)renderingArea.RenderLibHandle;
            var glContext = new Context(new DefaultGL(), new ContextInfra(tkContext));
            
            di.Bind<IContext>().AsLastChoice.To(glContext);
            infra = di.Get<IGraphicsInfra>();

            renderingRuntime = di.Get<IRenderingRuntime>();

            renderLoopDispatcher.Render += frameTime =>
            {
                renderingRuntime.RenderToContext(windowingSystem.RenderControl, frameTime.TotalSeconds);
            };
            renderLoopDispatcher.Closing += () =>
            {
                infra.MainThreadDisposer.FinishedWorking = true;
            };
        }

        public IImage CreateRenderTargetImage(IntSize2 size)
        {
            return new Ogl3TextureImage(ResourceVolatility.Stable, infra, size, false);
        }

        public void Render(IRenderGuiControl target, float timestamp)
        {
            renderingRuntime.RenderToContext(target, timestamp);
        }

        public void Render(IImage target, IReadOnlyList<IViewport> viewports, float timestamp)
        {
            if (!(target is IOgl3TextureImage glImage))
                throw new TypeContractException("IOgl3TextureImage is expected for render-to-texture using OGL3 RenderService");
            renderingRuntime.RenderToTexture(glImage.GlTexture, viewports, timestamp);
        }
    }
}