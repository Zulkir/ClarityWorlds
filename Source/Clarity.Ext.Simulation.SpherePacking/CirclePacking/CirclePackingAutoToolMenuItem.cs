using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public class CirclePackingAutoToolMenuItem : IToolMenuItem
    {
        public string Text => "CirclePackingAuto";

        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedoService;

        public CirclePackingAutoToolMenuItem(IToolService toolService, IUndoRedoService undoRedoService)
        {
            this.toolService = toolService;
            this.undoRedoService = undoRedoService;
        }

        public void OnActivate()
        {
            var entity = AmFactory.Create<SceneNode>();
            entity.Name = "CirclePacking";
            entity.Components.Add(AmFactory.Create<PresentationComponent>());
            entity.Components.Add(AmFactory.Create<CirclePackingAutoComponent>());
            var tool = new MoveEntityTool(entity, true, toolService, undoRedoService);
            toolService.CurrentTool = tool;
        }
    }
}