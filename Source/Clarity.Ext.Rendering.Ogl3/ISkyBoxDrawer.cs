using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface ISkyboxDrawer
    {
        void Draw(IDrawableTextureCube skyboxTexture, CameraFrame cameraFrame, float fieldOfView, float aspectRatio);
    }
}