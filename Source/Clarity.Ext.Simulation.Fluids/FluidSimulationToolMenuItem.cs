using Clarity.Core.AppCore.Tools;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.Ext.Simulation.Fluids
{
    public class FluidSimulationToolMenuItem : IToolMenuItem
    {
        public string Text => "Fluid";

        private readonly IToolService toolService;
        private readonly IViewService viewService;
        private readonly IUndoRedoService undoRedoService;
        private readonly ICommonNodeFactory commonNodeFactory;

        public FluidSimulationToolMenuItem(IToolService toolService, IViewService viewService, IUndoRedoService undoRedoService, ICommonNodeFactory commonNodeFactory)
        {
            this.toolService = toolService;
            this.viewService = viewService;
            this.undoRedoService = undoRedoService;
            this.commonNodeFactory = commonNodeFactory;
        }

        public void OnActivate()
        {
            var entity = AmFactory.Create<SceneNode>();
            entity.Components.Add(AmFactory.Create<FluidSimulationComponent>());
            var tool = new MoveEntityTool(entity, true, toolService, undoRedoService, commonNodeFactory);
            toolService.CurrentTool = tool;
        }
    }
}