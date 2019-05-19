using System;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Ext.Gui.EtoForms;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype
{
    public class PrototypeMainForm : Form, IMainForm
    {
        private readonly RenderControl renderControl;
        private readonly IPlayback playback;

        public Form Form => this;
        public RenderControl RenderControl => renderControl;

        public PrototypeMainForm(RenderControl renderControl, IPlayback playback, IRenderLoopDispatcher renderLoopDispatcher)
        {
            ClientSize = new Size(1280, 720);
            Title = "Clarity Transport Visualization";

            CreateMenuBar();

            this.renderControl = renderControl;
            var viewport = AmFactory.Create<Viewport>();
            viewport.View = AmFactory.Create<TransportView>();
            renderControl.SetViewports(new []{viewport}, new ViewportsLayout
            {
                ColumnWidths = new[]{new ViewportLength(100, ViewportLengthUnit.Percent)},
                RowHeights = new[]{new ViewportLength(100, ViewportLengthUnit.Percent)},
                ViewportIndices = new[,] {{0}}
            });

            renderLoopDispatcher.Update += f =>
            {
                viewport.View.Update(f);
            };

            this.playback = playback;
            renderControl.InitGraphics();
            //viewService.ChangeRenderingArea(renderControl);
            
            var playbackGui = new PlaybackGui(playback);
            var propertiesGui = new PropertiesGui(playback);

            playback.Updated += ft =>
            {
                playbackGui.Update(ft);
                propertiesGui.Update(ft);
            };

            var layout = new TableLayout(
                new TableRow(
                    new TableCell(new TableLayout(
                        new TableRow(renderControl){ScaleHeight = true},
                        playbackGui.Layout)) { ScaleWidth = true},
                    new TableCell(propertiesGui.Layout)
                )
            );
            
            Content = layout;
        }

        private void CreateMenuBar()
        {
            var menu = new MenuBar();

            var fileMenuItem = new ButtonMenuItem { Text = "&File" };
            var quitCommand = new Command((s, e) => Eto.Forms.Application.Instance.Quit())
            {
                MenuText = "&Quit"
            };

            var openCommand = new Command(OnOpenFile)
            {
                MenuText = "&Open",
                ToolBarText = "&Open",
            };

            fileMenuItem.Items.Add(openCommand);
            fileMenuItem.Items.AddSeparator();
            fileMenuItem.Items.Add(quitCommand);
            menu.Items.Add(fileMenuItem);

            Menu = menu;
        }

        private void OnOpenFile(object sender, EventArgs eventArgs)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filters.Add(new FileDialogFilter("Log", ".log", ".dlog"));
            openFileDialog.ShowDialog(this);
            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;
            playback.OpenFile(openFileDialog.FileName);
        }
    }
}