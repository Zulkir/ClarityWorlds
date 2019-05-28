using System;
using Clarity.App.Worlds.External.FluidSimulation;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Engine.Objects.WorldTree;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupMovieRectangle : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly Button cFile;
        private readonly Button cLink;
        private IFluidSimulationComponent boundComponent;

        public PropsGuiGroupMovieRectangle(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            cFile = new Button { Text = "File" };
            cFile.Click += OnFileClicked;

            cLink = new Button { Text = "Link" };
            cLink.Click += OnLinkClicked;
        }

        private void OnLinkClicked(object sender, EventArgs e)
        {

        }

        private void OnFileClicked(object sender, EventArgs e)
        {

        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = null;

            return node.HasComponent<MovieRectangleComponent>();
        }
    }
}