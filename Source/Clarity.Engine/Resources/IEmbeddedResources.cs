using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Models.Flexible.Embedded;
using Clarity.Engine.Media.Skyboxes;

namespace Clarity.Engine.Resources
{
    public interface IEmbeddedResources
    {
        IFlexibleModel LineModel();
        IFlexibleModel CircleModel(int numSegments);
        IFlexibleModel SimplePlaneXyModel();
        IFlexibleModel SimplePlaneXzModel();
        IFlexibleModel CubeModel();

        IFlexibleModel PlaneModel(PlaneModelSourcePlane plane, PlaneModelSourceNormalDirection normalDirection,
            float halfWidth, float halfHeight, float scaleU, float scaleV);

        IFlexibleModel Rect3DModel();
        IFlexibleModel SimpleFrustumModel();
        IFlexibleModel SphereModel(int halfNumCircleSegments, bool inverse = false);

        IImage Image(string path);
        ISkybox Skybox(string path);
    }
}