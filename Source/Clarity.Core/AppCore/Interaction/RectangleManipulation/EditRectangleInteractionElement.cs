using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.Interaction.RectangleManipulation
{
    public class EditRectangleInteractionElement : IInteractionElement
    {
        private readonly ISceneNodeBound master;
        private readonly ICommonNodeFactory commonNodeFactory;
        private ISceneNode gizmo;

        public EditRectangleInteractionElement(ISceneNodeBound master, ICommonNodeFactory commonNodeFactory)
        {
            this.master = master;
            this.commonNodeFactory = commonNodeFactory;
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (args is ICoreInterationEventArgs coreArgs)
                OnCoreInteractionEvent(coreArgs);
            return false;
        }

        private void OnCoreInteractionEvent(ICoreInterationEventArgs args)
        {
            if (args.Category != CoreInteractionEventCategory.PrimarySelection)
                return;
            if (args.Type == CoreInteractionEventType.Happened)
            {
                if (gizmo == null)
                    gizmo = commonNodeFactory.RectangleEditGizmo();
                else if (master.Node.ChildNodes.Contains(gizmo))
                    return;
                master.Node.ChildNodes.Add(gizmo);
            }
            else if (args.Type == CoreInteractionEventType.Released)
            {
                master.Node.ChildNodes.Remove(gizmo);
            }
        }
    }
}