using System;
using System.Drawing;
using Eto.Forms;
using Eto.WinForms.Forms;
using OpenTK.Graphics;
using OpenTK.Platform;

namespace Clarity.Ext.Gui.EtoForms
{
    public class RenderingAreaHandlerWinFormsOgl :
        WindowsControl<SelectablePanelWinForms, RenderControl, Control.ICallback>, 
        RenderControl.IHandler
    {
        private IWindowInfo windowInfo;
        private GraphicsContext graphicsContext;

        public object RenderLibHandle => graphicsContext;

        public RenderingAreaHandlerWinFormsOgl()
        {
            Control = new SelectablePanelWinForms
            {
                BackColor = Color.White,
            };
        }

        public event Action FullscreenEnded
        {
            add { Control.FullscreenEnded += value; }
            remove { Control.FullscreenEnded -= value; }
        }

        public void InitGraphics()
        {
            windowInfo = Utilities.CreateWindowsWindowInfo(Control.Handle);

            var colorFormat = new ColorFormat(32);
            var graphicsMode = new GraphicsMode(colorFormat, 24, 8);
            graphicsContext = new GraphicsContext(graphicsMode, windowInfo);
            graphicsContext.MakeCurrent(windowInfo);
            graphicsContext.LoadAll();
            graphicsContext.SwapInterval = 1;
        }

        public void PrepareForNewFrame() => 
            graphicsContext.MakeCurrent(windowInfo);

        public void Present() => 
            graphicsContext.SwapBuffers();

        public void GoFullscreen() => 
            Control.GoFullscreen();

        public void EndFullscreen() =>
            Control.EndFullscreen();
    }
}