using System;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Ext.Rendering.Ogl3.Drawers;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining 
{
    public class SceneRendererFactory : ISceneRendererFactory
    {
        private readonly IGraphicsInfra infra;
        private readonly IVisualElementHandlerContainer handlerContainer;
        private readonly ISkyboxDrawer skyboxDrawer;
        private readonly IBlurDrawer blurDrawer;
        private readonly ISketchDrawer sketchDrawer;
        private readonly IVeilDrawer veilDrawer;
        private readonly IHighlightDrawer highlightDrawer;

        public SceneRendererFactory(IGraphicsInfra infra, ISkyboxDrawer skyboxDrawer, 
            IVisualElementHandlerContainer handlerContainer, IBlurDrawer blurDrawer, 
            ISketchDrawer sketchDrawer, IVeilDrawer veilDrawer, IHighlightDrawer highlightDrawer)
        {
            this.infra = infra;
            this.skyboxDrawer = skyboxDrawer;
            this.handlerContainer = handlerContainer;
            this.blurDrawer = blurDrawer;
            this.sketchDrawer = sketchDrawer;
            this.veilDrawer = veilDrawer;
            this.highlightDrawer = highlightDrawer;
        }

        public ISceneRenderer Create(IPropertyBag pipelineRequirements)
        {
            var pipeline = new SceneRenderer(infra, skyboxDrawer, handlerContainer, blurDrawer, sketchDrawer, veilDrawer, highlightDrawer);
            if (!pipeline.SatisfiesRequirements(pipelineRequirements))
                throw new NotSupportedException("The specified requirements are not supported by this viewport layer rendering pipeline factory.");
            return pipeline;
        }
    }
}