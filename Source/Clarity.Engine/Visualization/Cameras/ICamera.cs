using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Visualization.Cameras
{
    public interface ICamera
    {
        // todo: remove from here
        Color4 VeilColor { get; }

        CameraProps GetProps();
        Vector3 GetEye();
        Vector3 GetTarget();
        Vector3 GetUp();
        float GetFov();
        CameraFrame GetGlobalFrame();
        CameraProjection GetProjectionProps();
        Matrix4x4 GetViewMat();
        Matrix4x4 GetProjectionMat(float aspectRatio);
        Matrix4x4 GetViewProjInverse(float aspectRatio);
        Ray3 GetGlobalRayForHmgnPointer(float aspectRatio, Vector2 hmgnPointerPos);

        void Update(FrameTime frameTime);
    }
}