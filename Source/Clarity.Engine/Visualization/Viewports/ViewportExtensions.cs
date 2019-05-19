using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Visualization.Viewports
{
    public static class ViewportExtensions
    {
        public static Ray3 GetGlobalRayForPixelPos(this IViewport viewport, IntVector2 pointerPixelPos)
        {
            // todo: use specific layer
            var camera = viewport.View.Layers[0].Camera;
            var hmgnPointerPos = new Vector2(
                -1f + 2f / viewport.Width * pointerPixelPos.X,
                1f - 2f / viewport.Height * pointerPixelPos.Y);
            return camera.GetGlobalRayForHmgnPointer(viewport.AspectRatio, hmgnPointerPos);
        }

        public static IntVector2 GetPixelPos(this IViewport viewport, ISceneNode node)
        {
            // todo: optimize

            //var visualNonuniformScale = node.MainVisual?.NonUniformScale ?? new Vector3(1, 1, 1);
            //var visualTransform = node.MainVisual?.Transform ?? Transform.Identity;
            //var worldMat = Matrix4x4.Scaling(visualNonuniformScale) *
            //            (visualTransform * node.GlobalTransform).ToMatrix4x4();

            var worldMat = node.GlobalTransform.ToMatrix4x4();

            // todo: use specific layer
            var camera = viewport.View.Layers[0].Camera;
            var viewFrame = camera.GetGlobalFrame();
            var viewMat = viewFrame.GetViewMat();
            var projMat = camera.GetProjectionMat(viewport.AspectRatio);
            var viewProjMat = viewMat * projMat;

            var worldViewProjMat = worldMat * viewProjMat;
            var hmgnCoord = new Vector4(0, 0, 0, 1) * worldViewProjMat;
            var normalizedCoord = (hmgnCoord / hmgnCoord.W).Xyz;
            return new IntVector2(
                (int)((normalizedCoord.X + 1f) / 2f * viewport.Width),
                (int)((-normalizedCoord.Y + 1f) / 2f * viewport.Height));
        }
    }
}