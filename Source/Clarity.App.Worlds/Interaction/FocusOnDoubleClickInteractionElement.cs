using Clarity.App.Worlds.Views;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction
{
    public class FocusOnDoubleClickInteractionElement : IInteractionElement
    {
        private readonly ISceneNodeBound master;

        public FocusOnDoubleClickInteractionElement(ISceneNodeBound master)
        {
            this.master = master;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            if (!(args is IMouseEvent cargs))
                return false;
            if (cargs.IsLeftDoubleClickEvent() && cargs.KeyModifyers == KeyModifyers.None)
            {
                (args.Viewport.View as IFocusableView)?.FocusOn(master.Node.GetComponent<IFocusNodeComponent>());
                return true;
            }
            return false;
        }
    }
}