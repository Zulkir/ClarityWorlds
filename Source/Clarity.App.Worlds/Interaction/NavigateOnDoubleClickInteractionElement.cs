using Clarity.App.Worlds.Navigation;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction 
{
    public class NavigateOnDoubleClickInteractionElement : IInteractionElement
    {
        private readonly ISceneNodeBound master;
        private readonly INavigationService navigationService;

        public NavigateOnDoubleClickInteractionElement(ISceneNodeBound master, INavigationService navigationService)
        {
            this.master = master;
            this.navigationService = navigationService;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            if (!(args is IMouseEvent cargs))
                return false;
            if (cargs.IsLeftDoubleClickEvent() && cargs.KeyModifyers == KeyModifyers.None)
            {
                navigationService.GoToSpecific(master.Node.Id);
                return true;
            }
            return false;
        }
    }
}