using System.Linq;
using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.StoryGraph.Editing.Flowchart;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;

namespace Clarity.App.Worlds.StoryGraph.Editing
{
    public class AddExplicitStoryGraphEdgeTool : ITool
    {
        private readonly IRayHitIndex rayHitIndex;
        private readonly IStoryService storyService;
        private readonly IToolService toolService;
        private readonly int from;

        public AddExplicitStoryGraphEdgeTool(int from, IRayHitIndex rayHitIndex, IToolService toolService, IStoryService storyService)
        {
            this.from = from;
            this.rayHitIndex = rayHitIndex;
            this.toolService = toolService;
            this.storyService = storyService;
        }

        public bool TryHandleInputEvent(IInputEvent eventArgs)
        {
            if (!(eventArgs is MouseEvent args))
                return false;
            if (!args.IsLeftClickEvent())
                return false;
            var clickInfo = new RayHitInfo(args.Viewport, args.Viewport.View.Layers.Single(), args.State.Position);
            var hitResult = rayHitIndex.CastRay(clickInfo).FirstOrNull() ?? RayHitResult.Failure();
            if (hitResult.Successful)
            {
                int? to = null;
                var gizmoComponentTo = hitResult.Node.SearchComponent<StoryFlowchartNodeGizmoComponent>();
                if (gizmoComponentTo != null && gizmoComponentTo.ReferencedNode.Id != from)
                    to = gizmoComponentTo.ReferencedNode.Id;

                if (storyService.GlobalGraph.NodeIds.Contains(hitResult.Node.Id))
                    to = hitResult.Node.Id;

                if (to.HasValue)
                {
                    storyService.AddEdge(from, to.Value);
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