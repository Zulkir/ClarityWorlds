using Clarity.Engine.Visualization.Cameras;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3.Drawers
{
    public interface ISkyboxDrawer
    {
        void Draw(ITextureCubemap gltextureCubemap, CameraFrame cameraFrame, float fieldOfView, float aspectRatio);
    }
}