using System;
using System.Linq;
using Clarity.App.Worlds.External.FluidSimulation;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupFluidSimulation : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly NumericUpDown widthControl;
        private readonly NumericUpDown heightControl;
        private readonly NumericUpDown levelSetScaleControl;
        private readonly NumericUpDown cellSizeControl;
        private readonly DropDown surfaceTypeDropDown;
        private IFluidSimulationComponent boundComponent;

        public PropsGuiGroupFluidSimulation(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            widthControl = new NumericUpDown();
            heightControl = new NumericUpDown();
            levelSetScaleControl = new NumericUpDown();
            cellSizeControl = new NumericUpDown();
            surfaceTypeDropDown = new DropDown
            {
                Width = 120,
                DataStore = new[]
                {
                    FluidSurfaceType.Hybrid,
                    FluidSurfaceType.Particles,
                    FluidSurfaceType.LevelSet
                }.Select(x => (object)x)
            };

            widthControl.ValueChanged += OnChanged;
            heightControl.ValueChanged += OnChanged;
            levelSetScaleControl.ValueChanged += OnChanged;
            cellSizeControl.ValueChanged += OnChanged;
            surfaceTypeDropDown.SelectedValueChanged += OnChanged;

            var layout = new TableLayout(
                new TableRow(new Label { Text = "Width" }, widthControl),
                new TableRow(new Label { Text = "Height" }, heightControl),
                new TableRow(new Label { Text = "LSScale" }, levelSetScaleControl),
                new TableRow(new Label { Text = "CellSize" }, cellSizeControl),
                new TableRow(new Label { Text = "Type" }, surfaceTypeDropDown)
                )
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            GroupBox = new GroupBox
            {
                Text = "Fluid",
                Content = layout
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = null;

            var newNode = node.SearchComponent<IFluidSimulationComponent>();
            if (newNode == null)
                return false;

            widthControl.Value = newNode.Width;
            heightControl.Value = newNode.Height;
            levelSetScaleControl.Value = newNode.LevelSetScale;
            cellSizeControl.Value = (int)(newNode.CellSize * 10);
            surfaceTypeDropDown.SelectedValue = newNode.SurfaceType;

            boundComponent = newNode;
            return true;
        }

        private void OnChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            boundComponent.Width = (int)widthControl.Value;
            boundComponent.Height = (int)heightControl.Value;
            boundComponent.LevelSetScale = (int)levelSetScaleControl.Value;
            boundComponent.CellSize = (float)(cellSizeControl.Value / 10f);
            boundComponent.SurfaceType = (FluidSurfaceType)surfaceTypeDropDown.SelectedValue;
            undoRedo.OnChange();
        }
    }
}