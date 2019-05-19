using System.Diagnostics;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.Engine.Interaction.RayHittables
{
    public class RayHitInfo
    {
        public IScene Scene { get; }
        public Ray3 GlobalRay { get; }
        public Matrix4x4 ViewProjection { get; }
        public IntSize2 ViewportSize { get; }
        public float AspectRatio { get; }

        public RayHitInfo(IViewport viewport, IViewLayer layer, IntVector2 pointerPixelPos)
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
            var hmgnPointerPos = new Vector2(
                -1f + 2f / ViewportSize.Width * pointerPixelPos.X,
                1f - 2f / ViewportSize.Height * pointerPixelPos.Y);
            GlobalRay = camera.GetGlobalRayForHmgnPointer(AspectRatio, hmgnPointerPos);
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