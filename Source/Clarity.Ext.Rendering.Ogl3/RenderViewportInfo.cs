using Clarity.Common.Numericals;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;
using ObjectGL.Api.Context.States.ScreenClipping;

namespace Clarity.Ext.Rendering.Ogl3
{
    public struct RenderViewportInfo
    {
        public IScene Scene;
        public ScissorBox ScissorBox;
        public ViewportInt OglViewport;
        public ICamera Camera;
        public float AspectRatio;

        public RenderViewportInfo(IViewport viewport, IViewLayer layer)
        {
            Scene = layer.VisibleScene;
            ScissorBox = new ScissorBox
            {
                X = viewport.Left,
                Y = viewport.Top,
                Width = viewport.Width,
                Height = viewport.Height
            };
            OglViewport = new ViewportInt
            {
                X = viewport.Left,
                Y = viewport.Top,
                Width = viewport.Width,
                Height = viewport.Height
            };
            Camera = layer.Camera;
            AspectRatio = viewport.AspectRatio;
        }

        public RenderViewportInfo(int width, int height, IScene scene, ICamera camera)
        {
            Scene = scene;
            ScissorBox = new ScissorBox
            {
                X = 0,
                Y = 0,
                Width = width,
                Height = height
            };
            OglViewport = new ViewportInt
            {
                X = 0,
                Y = 0,
                Width = width,
                Height = height
            };
            Camera = camera;
            AspectRatio = GraphicsHelper.AspectRatio(width, height);
        }
    }
}