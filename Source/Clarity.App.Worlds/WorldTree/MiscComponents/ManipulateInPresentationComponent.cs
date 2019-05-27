using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Views;
using Clarity.Engine.Gui;
using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.WorldTree.MiscComponents
{
    // todo: create an interface for this
    public abstract class ManipulateInPresentationComponent : SceneNodeComponentBase<ManipulateInPresentationComponent>, 
        IInteractionComponent, IGuiComponent
    {
        private readonly IAppModeService appModeService;
        private readonly IPresentationGuiCommands commands;
        private readonly IViewService viewService;

        protected ManipulateInPresentationComponent(IAppModeService appModeService, IViewService viewService, IPresentationGuiCommands commands)
        {
            this.appModeService = appModeService;
            this.viewService = viewService;
            this.commands = commands;
        }

        private bool TryHandleMouseEvent(IMouseEventArgs eventArgs)
        {
            if (eventArgs.ComplexEventType == MouseEventType.Click && eventArgs.EventButtons == MouseButtons.Left)
            {
                viewService.SelectedNode = Node;
            }
            return false;
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (appModeService.Mode == AppMode.Presentation && args is IMouseEventArgs margs && margs.IsClickEvent())
            {
                TryHandleMouseEvent(margs);
                return true;
            }
            return false;
        }

        public void BuildMenuSection(IGuiMenuBuilder menuBuilder)
        {
            menuBuilder.StartSection();
            menuBuilder.AddCommand(commands.Move);
            menuBuilder.AddCommand(commands.Rotate);
            menuBuilder.AddCommand(commands.Scale);
        }
    }
}
