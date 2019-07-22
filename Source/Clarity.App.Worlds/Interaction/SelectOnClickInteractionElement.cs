using Clarity.App.Worlds.Views;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction
{
    public class SelectOnClickInteractionElement : IInteractionElement
    {
        private readonly IViewService viewService;
        private readonly ISceneNodeBound nodeBound;

        public SelectOnClickInteractionElement(ISceneNodeBound nodeBound, IViewService viewService)
        {
            this.nodeBound = nodeBound;
            this.viewService = viewService;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            if (!(args is IMouseEvent cargs))
                return false;
            if ((cargs.IsLeftClickEvent() || cargs.IsRightClickEvent()) && cargs.KeyModifyers == KeyModifyers.None)
                viewService.SelectedNode = nodeBound.Node;
            return false;
        }
    }
}