using Clarity.App.Worlds.Helpers;
using Clarity.Engine.Interaction;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction.RectangleManipulation
{
    public class EditRectangleInteractionElement : IInteractionElement
    {
        private readonly ISceneNodeBound master;
        private readonly ICommonNodeFactory commonNodeFactory;
        private IScene scene;
        private ISceneNode gizmo;

        public EditRectangleInteractionElement(ISceneNodeBound master, ICommonNodeFactory commonNodeFactory)
        {
            this.master = master;
            this.commonNodeFactory = commonNodeFactory;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            if (args is ICoreInterationEvent coreArgs)
                OnCoreInteractionEvent(coreArgs);
            return false;
        }

        private void OnCoreInteractionEvent(ICoreInterationEvent args)
        {
            if (args.Category != CoreInteractionEventCategory.PrimarySelection)
                return;
            if (args.Type == CoreInteractionEventType.Happened)
            {
                if (scene == null)
                    scene = master.Node.Scene;
                if (gizmo == null)
                    gizmo = commonNodeFactory.RectangleEditGizmo(master.Node);
                else if (scene.AuxuliaryNodes.Contains(gizmo))
                    return;
                scene.AuxuliaryNodes.Add(gizmo);
            }
            else if (args.Type == CoreInteractionEventType.Released)
            {
                scene.AuxuliaryNodes.Remove(gizmo);
            }
        }
    }
}