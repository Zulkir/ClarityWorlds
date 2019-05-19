using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Skyboxes
{
    public interface ISkybox : IResource
    {
        int Width { get; }
        byte[] GetRawData(SkyboxFace skyboxFace);
    }
}