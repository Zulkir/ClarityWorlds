using System.Diagnostics;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.Engine.Interaction.RayHittables
{
    public class RayCastInfo
    {
        public IScene Scene { get; private set; }
        public Ray3 GlobalRay { get; private set; }
        public Matrix4x4 ViewProjection { get; private set; }
        public IntSize2 ViewportSize { get; private set; }
        public float AspectRatio { get; private set; }

        public RayCastInfo(IViewport viewport, IViewLayer layer, IntVector2 pointerPixelPos)
        {
            InitViewProperties(viewport, layer);
            GlobalRay = GenGlobalRayFromMousePos(layer.Camera, ViewportSize, AspectRatio, pointerPixelPos);
        }

        public RayCastInfo(IViewport viewport, IViewLayer layer, Ray3 globalRay)
        {
            InitViewProperties(viewport, layer);
            GlobalRay = globalRay;
        }

        private static Ray3 GenGlobalRayFromMousePos(ICamera camera, IntSize2 viewportSize, float aspectRatio, IntVector2 pointerPixelPos)
        {
            var hmgnPointerPos = new Vector2(
                -1f + 2f / viewportSize.Width * pointerPixelPos.X,
                1f - 2f / viewportSize.Height * pointerPixelPos.Y);
            return camera.GetGlobalRayForHmgnPointer(aspectRatio, hmgnPointerPos);
        }

        private void InitViewProperties(IViewport viewport, IViewLayer layer)
        {
            var view = viewport.View;
            Debug.Assert(view != null, nameof(view) + " != null");
            Scene = layer.VisibleScene;
            ViewportSize = new IntSize2(viewport.Width, viewport.Height);
            AspectRatio = viewport.AspectRatio;
            var camera = layer.Camera;
            var viewMat = camera.GetViewMat();
            var projMat = camera.GetProjectionMat(AspectRatio);
            ViewProjection = viewMat * projMat;
        }

        public Vector3 GetAbsPixelGradientAt(Vector3 point)
        {
            var hmgnPoint = ApplyViewProj(point);
            var hmgnPointDeltaX = ApplyViewProj(point + Vector3.UnitX);
            var hmgnPointDeltaY = ApplyViewProj(point + Vector3.UnitY);
            var hmgnPointDeltaZ = ApplyViewProj(point + Vector3.UnitZ);
            return new Vector3(
                (hmgnPointDeltaX.Xy - hmgnPoint.Xy).Length(),
                (hmgnPointDeltaY.Xy - hmgnPoint.Xy).Length(),
                (hmgnPointDeltaZ.Xy - hmgnPoint.Xy).Length());
        }

        private Vector3 ApplyViewProj(Vector3 point)
        {
            var raw = new Vector4(point, 1f) * ViewProjection;
            return raw.Xyz / raw.W;
        }
    }
}