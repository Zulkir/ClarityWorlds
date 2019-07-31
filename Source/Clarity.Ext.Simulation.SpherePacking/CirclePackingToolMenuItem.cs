using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.Ext.Simulation.SpherePacking 
{
    public class CirclePackingToolMenuItem : IToolMenuItem
    {
        public string Text => "CirclePacking";

        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedoService;

        public CirclePackingToolMenuItem(IToolService toolService, IUndoRedoService undoRedoService)
        {
            this.toolService = toolService;
            this.undoRedoService = undoRedoService;
        }

        public void OnActivate()
        {
            var entity = AmFactory.Create<SceneNode>();
            entity.Name = "CirclePacking";
            entity.Components.Add(AmFactory.Create<PresentationComponent>());
            entity.Components.Add(AmFactory.Create<CirclePackingNodeComponent>());
            var tool = new MoveEntityTool(entity, true, toolService, undoRedoService);
            toolService.CurrentTool = tool;
        }
    }
}