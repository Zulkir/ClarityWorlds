using Clarity.Common.CodingUtilities.Collections;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining
{
    public interface ISceneRendererPool
    {
        ISceneRenderer Allocate(IPropertyBag pipelineRequirements);
        void Return(ISceneRenderer pipeline);
    }
}