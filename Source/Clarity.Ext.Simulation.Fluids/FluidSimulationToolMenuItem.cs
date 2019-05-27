using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.Ext.Simulation.Fluids
{
    public class FluidSimulationToolMenuItem : IToolMenuItem
    {
        public string Text => "Fluid";

        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedoService;

        public FluidSimulationToolMenuItem(IToolService toolService, IUndoRedoService undoRedoService)
        {
            this.toolService = toolService;
            this.undoRedoService = undoRedoService;
        }

        public void OnActivate()
        {
            var entity = AmFactory.Create<SceneNode>();
            entity.Components.Add(AmFactory.Create<FluidSimulationComponent>());
            var tool = new MoveEntityTool(entity, true, toolService, undoRedoService);
            toolService.CurrentTool = tool;
        }
    }
}