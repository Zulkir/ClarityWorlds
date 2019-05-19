using System;
using Clarity.Native.Linux;
using Eto.Forms;
using Eto.GtkSharp.Forms;
using Gtk;
using OpenTK.Graphics;
using OpenTK.Platform;

namespace Clarity.Ext.Gui.EtoForms
{
    public class RenderingAreaHandlerGtkOgl : GtkControl<DrawingArea, RenderControl, Control.ICallback>, RenderControl.IHandler
    {
        private IWindowInfo windowInfo;
        private GraphicsContext graphicsContext;

        public ContextMenu ContextMenu { get; set; }

        public RenderingAreaHandlerGtkOgl()
        {
            Control = new DrawingArea();
        }

        public event System.Action FullscreenEnded
        {
            add {  }
            remove {  }
        }

        public object RenderLibHandle
        {
            get { return graphicsContext; }
        }

        public void InitGraphics()
        {
            var colorFormat = new ColorFormat(32);
            var graphicsMode = new GraphicsMode(colorFormat, 24, 8);

            var display = GdkApi.gdk_x11_display_get_xdisplay(Control.Display.Handle);
            var screen = Control.Screen.Number;
            var windowHandle = GdkApi.gdk_x11_drawable_get_xid(Control.GdkWindow.Handle);
            var rootWindow = GdkApi.gdk_x11_drawable_get_xid(Control.RootWindow.Handle);

            IntPtr visualInfo;
            if (graphicsMode.Index.HasValue)
            {
                var xVisualInfo = new XVisualInfo
                {
                    VisualID = graphicsMode.Index.Value
                };
                int dummy;
                visualInfo = XApi.XGetVisualInfo(display, (IntPtr)XVisualInfoMask.ID, ref xVisualInfo, out dummy);
            }
            else
            {
                visualInfo = GlxApi.glXChooseVisual(display, Control.Screen.Number, new[] {
                    GlxApi.GLX_RGBA,
                    GlxApi.GLX_DOUBLEBUFFER,
                    GlxApi.GLX_RED_SIZE, 8,
                    GlxApi.GLX_GREEN_SIZE, 8,
                    GlxApi.GLX_BLUE_SIZE, 8,
                    GlxApi.GLX_ALPHA_SIZE, 8,
                    GlxApi.GLX_DEPTH_SIZE, 24,
                    GlxApi.GLX_STENCIL_SIZE, 8,
                    GlxApi.GLX_NONE
                });
            }

            windowInfo = Utilities.CreateX11WindowInfo(display, screen, windowHandle, rootWindow, visualInfo);

            graphicsContext = new GraphicsContext(graphicsMode, windowInfo);
            graphicsContext.MakeCurrent(windowInfo);
            graphicsContext.LoadAll();
            graphicsContext.SwapInterval = 1;
        }

        public void PrepareForNewFrame()
        {
            graphicsContext.MakeCurrent(windowInfo);
        }

        public void Present()
        {
            graphicsContext.SwapBuffers();
        }

        public void GoFullscreen()
        {
            throw new NotImplementedException();
        }

        public void EndFullscreen()
        {
            throw new NotImplementedException();
        }
    }
}