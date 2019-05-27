using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Collections;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining 
{
    public class SceneRendererPool : ISceneRendererPool
    {
        private readonly ISceneRendererFactory factory;
        private readonly List<ISceneRenderer> reserve;

        public SceneRendererPool(ISceneRendererFactory factory)
        {
            this.factory = factory;
            reserve = new List<ISceneRenderer>();
        }

        public ISceneRenderer Allocate(IPropertyBag pipelineRequirements)
        {
            for (var i = reserve.Count - 1; i >= 0; i--)
                if (reserve[i].SatisfiesRequirements(pipelineRequirements))
                {
                    var pipeline = reserve[i];
                    reserve.RemoveAt(i);
                    return pipeline;
                }
            return factory.Create(pipelineRequirements);
        }

        public void Return(ISceneRenderer pipeline)
        {
            reserve.Add(pipeline);
        }
    }
}