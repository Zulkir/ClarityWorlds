using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.StoryGraph
{
    public class StoryGraphGui : IStoryGraphGui
    {
        public Command ViewStoryGraphCommand { get; }

        public StoryGraphGui(IViewService viewService, IStoryService storyService, IWorldTreeService worldTreeService)
        {
            ViewStoryGraphCommand = new Command(
                (s, a) =>
                {
                    viewService.MainView.FocusOn((viewService.MainView.FocusNode.Scene != storyService.EditingScene 
                        ? storyService.EditingScene.Root
                        : worldTreeService.MainRoot).GetComponent<IFocusNodeComponent>());
                })
            {
                MenuText = "Story &Graph",
                ToolBarText = "Story &Graph",
                Shortcut = Keys.F4
            };
        }
    }
}