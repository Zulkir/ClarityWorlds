using Clarity.Common.CodingUtilities.Collections;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Ext.Rendering.Ogl3.Helpers;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining
{
    public interface ISceneRenderer
    {
        bool SatisfiesRequirements(IPropertyBag pipelineRequirements);
        void Execute(IScene scene, ICamera camera, IOffScreen offScreen, float timestamp, IPropertyBag settings);
    }
}