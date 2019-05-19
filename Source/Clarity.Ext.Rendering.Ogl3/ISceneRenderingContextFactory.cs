using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface ISceneRenderingContextFactory
    {
        ISceneRenderingContext Create(ISceneRenderingContext parentContext, ICamera camera, float aspectRatio);
    }
}