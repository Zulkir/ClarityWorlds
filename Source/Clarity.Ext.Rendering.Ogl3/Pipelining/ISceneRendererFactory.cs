using Clarity.Common.CodingUtilities.Collections;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining
{
    public interface ISceneRendererFactory
    {
        ISceneRenderer Create(IPropertyBag pipelineRequirements);
    }
}