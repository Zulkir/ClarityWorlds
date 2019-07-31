using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Explicit;
using Clarity.Engine.Media.Models.Explicit.Embedded;
using Clarity.Engine.Media.Skyboxes;

namespace Clarity.Engine.Resources
{
    public interface IEmbeddedResources
    {
        IExplicitModel LineModel();
        IExplicitModel CircleModel(int numSegments);
        IExplicitModel RectangleModel();
        IExplicitModel SimplePlaneXyModel();
        IExplicitModel SimplePlaneXzModel();
        IExplicitModel CubeModel();

        IExplicitModel PlaneModel(PlaneModelSourcePlane plane, PlaneModelSourceNormalDirection normalDirection,
            float halfWidth, float halfHeight, float scaleU, float scaleV);

        IExplicitModel SimpleFrustumModel();
        IExplicitModel SphereModel(int halfNumCircleSegments, bool inverse = false);

        IImage Image(string path);
        ISkybox Skybox(string path);
    }
}