using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Gui;
using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Visualization.Viewports;
using Eto;
using Eto.Drawing;
using Eto.Forms;
using OpenTK.Graphics.OpenGL4;
using KeyEventArgs = Eto.Forms.KeyEventArgs;
using MouseEventArgs = Eto.Forms.MouseEventArgs;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace Clarity.Ext.Gui.EtoForms
{
    [Handler(typeof(IHandler))]
    public class RenderControl : Control, IRenderGuiControl
    {
        public new interface IHandler : Control.IHandler, IContextMenuHost
        {
            object RenderLibHandle { get; }
            event Action FullscreenEnded;
            void PrepareForNewFrame();
            void InitGraphics();
            void Present();
            void GoFullscreen();
            void EndFullscreen();
        }
        
        private readonly IInputService inputService;

        public IReadOnlyList<IViewport> Viewports { get; private set; }
        private ViewportsLayout tableLayout;

        public IReadOnlyList<IGuiCommand> Commands { get; private set; }

        public event Action Resized;
        
        public RenderControl(IInputService inputService, ICommonGuiObjects commonGuiObjects)
        {
            this.inputService = inputService;
            Viewports = new IViewport[0];
            ContextMenu = commonGuiObjects.SelectionContextMenu;
            tableLayout = new ViewportsLayout
            {
                RowHeights = EmptyArrays<ViewportLength>.Array,
                ColumnWidths = EmptyArrays<ViewportLength>.Array,
                ViewportIndices = new int[0,0]
            };
        }

        public event Action FullscreenEnded
        {
            add => Handler.FullscreenEnded += value;
            remove => Handler.FullscreenEnded -= value;
        }

        private new IHandler Handler => (IHandler)base.Handler;
        public object RenderLibHandle => Handler.RenderLibHandle;

        public void InitGraphics() => Handler.InitGraphics();
        public void PrepareForNewFrame() => Handler.PrepareForNewFrame();
        public void Present() => Handler.Present();

        public ContextMenu ContextMenu
        {
            get => Handler.ContextMenu;
            set => Handler.ContextMenu = value;
        }

        public void GoFullscreen()
        {
            Handler.GoFullscreen();
        }

        public void EndFullscreen()
        {
            Handler.EndFullscreen();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ResizeViewports();
            Resized?.Invoke();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var viewport = GetViewport(new Vector2(e.Location.X, e.Location.Y));
            inputService.OnFocusedViewportChanged(viewport);
            base.OnMouseDown(e);
        }

        public IViewport GetViewport(Vector2 mousePoint)
        {
            return 
                Viewports.FirstOrDefault(x => new AaRectangle2(new Vector2(x.Left, x.Top), new Size2(x.Width, x.Height)).ContainsPoint(mousePoint))
                ?? Viewports.FirstOrDefault();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            inputService.OnFocusedViewportChanged(null);
            base.OnLostFocus(e);
        }

        protected override unsafe void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Keys.F12)
            {
                // todo: move to RenderingSystem
                var bitmap = new Bitmap(new Size(Width, Height), Eto.Drawing.PixelFormat.Format32bppRgb);
                GL.GetInteger(GetPName.ReadFramebufferBinding, out var fbuf);
                GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
                using (var bData = bitmap.Lock())
                {
                    GL.ReadPixels(0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, bData.Data);
                    for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                    {
                        byte* pixel = (byte*)bData.Data + bData.ScanWidth * y + 4 * x;
                        var r = pixel[2];
                        pixel[2] = pixel[0];
                        pixel[0] = r;
                    }

                    for (int y = 0; y < Height / 2; y++)
                    for (int x = 0; x < Width; x++)
                    {
                        int* pixelUp = (int*)((byte*)bData.Data + bData.ScanWidth * y + 4 * x);
                        int* pixelDown = (int*)((byte*)bData.Data + bData.ScanWidth * (Height - y - 1) + 4 * x);
                        int rgba = *pixelUp;
                        *pixelUp = *pixelDown;
                        *pixelDown = rgba;
                    }
                }
                GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, fbuf);

                int i = 0;
                string fileName;
                do
                {
                    fileName =
                        i < 10
                            ? $"Screenshots/Screen00{i}.png"
                            : i < 100
                                ? $"Screenshots/Screen0{i}.png"
                                : $"Screenshots/Screen{i}.png";
                    i++;
                } while (File.Exists(fileName));

                if (!Directory.Exists("Screenshots"))
                    Directory.CreateDirectory("Screenshots");
                bitmap.Save(fileName, ImageFormat.Png);
            }
            
            base.OnKeyDown(e);
        }

        public void SetCommands(IReadOnlyList<IGuiCommand> commands)
        {
            Commands = commands;
            ContextMenu = commands != null && commands.Count != 0
                ? new ContextMenu(commands.Select(x => new ButtonMenuItem(new Command((s, a) => x.Execute())
                {
                    MenuText = x.Text,
                    Shortcut = Converters.ToEto(x.ShortcutKey, x.ShortcutModifiers)
                })))
                : null;
        }
        
        public void SetViewports(IReadOnlyList<IViewport> viewports, ViewportsLayout layout)
        {
            tableLayout = layout;
            Viewports = viewports;
            ResizeViewports();
            if (HasFocus)
                inputService.OnFocusedViewportChanged(Viewports.FirstOrDefault());
        }

        private void ResizeViewports()
        {
            var numRows = tableLayout.ViewportIndices.GetLength(0);
            var numCols = tableLayout.ViewportIndices.GetLength(1);
            var offsets = new Vector2[numRows + 1, numCols + 1];
            var rowOffset = 0f;
            for (int r = 0; r < numRows + 1; r++)
            {
                var colOffset = 0f;
                for (int c = 0; c < numCols + 1; c++)
                {
                    offsets[r, c] = new Vector2(colOffset, rowOffset);
                    if (c < numCols)
                        colOffset += ToActualPixels(tableLayout.ColumnWidths[c], Width);
                }
                if (r < numRows)
                    rowOffset += ToActualPixels(tableLayout.RowHeights[r], Height);
            }

            for (int i = 0; i < Viewports.Count; i++)
            {
                var viewport = Viewports[i];
                if (TryGetViewportBorderCells(i, out var startRow, out var endRow, out var startCol, out var endCol))
                {
                    var topLeft = offsets[startRow, startCol];
                    var bottomRight = offsets[endRow + 1, endCol + 1];
                    var size = bottomRight - topLeft;
                    viewport.OnResized(topLeft.X, topLeft.Y, size.X, size.Y);
                }
                else
                {
                    viewport.OnResized(0, 0, 0, 0);
                }
            }
        }

        private static float ToActualPixels(ViewportLength viewportLength, int pixelLength)
        {
            switch (viewportLength.Unit)
            {
                case ViewportLengthUnit.Pixels:
                case ViewportLengthUnit.ScaledPixels: return viewportLength.Value;
                case ViewportLengthUnit.Percent: return viewportLength.Value * pixelLength / 100;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private bool TryGetViewportBorderCells(int viewportIndex, out int startRow, out int endRow, out int startCol, out int endCol)
        {
            startRow = startCol = int.MaxValue;
            endRow = endCol = int.MinValue;

            var numRows = tableLayout.ViewportIndices.GetLength(0);
            var numCols = tableLayout.ViewportIndices.GetLength(1);
            for (int r = 0; r < numRows; r++)
            for (int c = 0; c < numCols; c++)
            {
                if (tableLayout.ViewportIndices[r, c] != viewportIndex)
                    continue;
                if (startRow > r)
                    startRow = r;
                if (startCol > c)
                    startCol = c;
                if (endRow < r)
                    endRow = r;
                if (endCol < c)
                    endCol = c;
            }

            return startRow != int.MaxValue && startCol != int.MaxValue;
        }
    }
}