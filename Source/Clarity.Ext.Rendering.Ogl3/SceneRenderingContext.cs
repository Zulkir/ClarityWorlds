using System.Collections.Generic;
using System.Linq;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class SceneRenderingContext : ISceneRenderingContext
    {
        public ISceneRenderingContext ParentContext { get; }
        public IGraphicsInfra Infra { get; }
        public IReadOnlyList<IRenderStage> Stages { get; }
        public IReadOnlyDictionary<string, IRenderStage> StagesByName { get; }
        public ICamera Camera { get; }
        public float AspectRatio { get; }

        public SceneRenderingContext(ISceneRenderingContext parentContext, IGraphicsInfra infra, IReadOnlyList<IRenderStage> stages, ICamera camera, float aspectRatio)
        {
            ParentContext = parentContext;
            Infra = infra;
            Stages = stages;
            StagesByName = Stages.ToDictionary(x => x.Name);
            Camera = camera;
            AspectRatio = aspectRatio;
        }

        public void Clear()
        {
            foreach (var wave in Stages)
                wave.Queue?.Clear();
        }

        public ISceneNode CurrentTraverseNode { get; set; }
        public IRenderStage CurrentStage { get; set; }
    }
}