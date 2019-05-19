using System.Linq;
using Clarity.Core.AppCore.StoryGraph.Editing.Flowchart;
using Clarity.Core.AppCore.Tools;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;

namespace Clarity.Core.AppCore.StoryGraph.Editing
{
    public class StoryBranchIntoTool : ITool
    {
        private readonly IRayHitIndex rayHitIndex;
        private readonly IStoryService storyService;
        private readonly IToolService toolService;
        private readonly ICommonNodeFactory commonNodeFactory;
        private readonly int from;

        public StoryBranchIntoTool(int from, IRayHitIndex rayHitIndex, IToolService toolService, IStoryService storyService, ICommonNodeFactory commonNodeFactory)
        {
            this.from = from;
            this.rayHitIndex = rayHitIndex;
            this.toolService = toolService;
            this.storyService = storyService;
            this.commonNodeFactory = commonNodeFactory;
        }

        public bool TryHandleInputEvent(IInputEventArgs eventArgs)
        {
            if (!(eventArgs is MouseEventArgs args))
                return false;
            if (!args.IsLeftClickEvent())
                return false;
            var clickInfo = new RayHitInfo(args.Viewport, args.Viewport.View.Layers.Single(), args.State.Position);
            var hitResult = rayHitIndex.FindEntity(clickInfo);
            if (hitResult.Successful)
            {
                int? to = null;
                var gizmoComponentTo = hitResult.Node.GetComponent<StoryFlowchartNodeGizmoComponent>();
                if (gizmoComponentTo != null && gizmoComponentTo.ReferencedNode.Id != from)
                    to = gizmoComponentTo.ReferencedNode.Id;

                if (storyService.GlobalGraph.NodeIds.Contains(hitResult.Node.Id))
                    to = hitResult.Node.Id;
                
                if (to.HasValue)
                {
                    var child = StoryOperations.AddChild(storyService, commonNodeFactory, to.Value, this);
                    storyService.AddEdge(from, child.Id);
                    toolService.CurrentTool = null;
                    return true;
                }
            }
            toolService.CurrentTool = null;
            return false;
        }

        public void Dispose()
        {
        }
    }
}