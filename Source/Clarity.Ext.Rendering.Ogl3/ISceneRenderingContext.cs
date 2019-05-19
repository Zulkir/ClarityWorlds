using System.Collections.Generic;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface ISceneRenderingContext
    {
        ISceneRenderingContext ParentContext { get; }
        IGraphicsInfra Infra { get; }
        IReadOnlyList<IRenderStage> Stages { get; }
        IReadOnlyDictionary<string, IRenderStage> StagesByName { get; }
        ICamera Camera { get; }
        float AspectRatio { get; }
        void Clear();

        ISceneNode CurrentTraverseNode { get; set; }
        IRenderStage CurrentStage { get; set; }
    }
}