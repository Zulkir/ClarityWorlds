using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Core.AppCore.Tools
{
    public class MoveEntityTool : ITool
    {
        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedo;
        private readonly ICommonNodeFactory commonNodeFactory;

        private readonly ISceneNode entity;
        private readonly Transform initialLocalTransform;
        private readonly bool isNew;
        
        private bool done;

        public MoveEntityTool(ISceneNode entity, bool isNew, IToolService toolService, IUndoRedoService undoRedo, ICommonNodeFactory commonNodeFactory)
        {
            this.entity = entity;
            this.isNew = isNew;
            initialLocalTransform = entity.Transform;
            this.toolService = toolService;
            this.undoRedo = undoRedo;
            this.commonNodeFactory = commonNodeFactory;
            done = false;
        }

        public bool TryHandleInputEvent(IInputEventArgs eventArgs)
        {
            return eventArgs is IMouseEventArgs mouseArgs && TryHandleMouseEvent(mouseArgs);
        }

        private bool TryHandleMouseEvent(IMouseEventArgs eventArgs)
        {
            var viewport = eventArgs.Viewport;
            var focusedNode = (viewport.View as IFocusableView)?.FocusNode;
            if (focusedNode == null)
                return false;

            var placementAspects = focusedNode.ChildNodes.Select(x => x.SearchComponent<IPlacementPlaneComponent>()).Where(x => !(x?.Is2D ?? false)).ToArray();

            if (!placementAspects.Any())
            {
                var placementNode = commonNodeFactory.PlacementPlane3D();
                undoRedo.Common.Add(focusedNode.ChildNodes, placementNode);
                placementAspects = new[] {placementNode.GetComponent<IPlacementPlaneComponent>()};
            }

            var placeExists = false;
            var globalRay = viewport.GetGlobalRayForPixelPos(eventArgs.State.Position);
            foreach (var aPlacement in placementAspects)
            {
                if (!aPlacement.PlacementPlane.TryFindPlace(globalRay, out var placementTransform))
                {
                    continue;
                }

                var newTransform = new Transform
                {
                    Scale = initialLocalTransform.Scale * placementTransform.Scale,
                    Rotation = initialLocalTransform.Rotation * placementTransform.Rotation,
                    Offset = placementTransform.Offset
                };

                if (isNew)
                    aPlacement.Node.ChildNodes.AddUnique(entity);

                if (eventArgs.ComplexEventType == MouseEventType.Click && eventArgs.EventButtons == MouseButtons.Left)
                {
                    if (isNew)
                    {
                        entity.Deparent();
                        undoRedo.Common.Add(aPlacement.Node.ChildNodes, entity);
                    }
                    else
                    {
                        entity.Transform = initialLocalTransform;
                        undoRedo.Common.ChangeProperty(entity, x => x.Transform, newTransform);
                    }
                    done = true;
                    toolService.CurrentTool = null;
                }
                else
                {
                    entity.Transform = newTransform;
                }

                placeExists = true;
                break;
            }
            
            if (!placeExists)
                entity.Deparent();

            return false;
        }

        public void Dispose()
        {
            if (done)
                return;
            if (isNew)
                entity.Deparent();
            else
                entity.Transform = initialLocalTransform;
        }
    }
}