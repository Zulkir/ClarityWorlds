using Clarity.Core.AppCore.Views;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.Interaction 
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

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (!(args is IMouseEventArgs cargs))
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