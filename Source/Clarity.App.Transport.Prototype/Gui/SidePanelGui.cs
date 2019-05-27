using Clarity.App.Transport.Prototype.Gui.Tables;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Engine.Platforms;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype.Gui
{
    public class SidePanelGui
    {
        public Panel Control { get; }

        private readonly TablesGui tablesGui;
        private readonly UserDataQueriesGui userDataQueriesGui;

        public SidePanelGui(IAppRuntime appRuntime)
        {
            tablesGui = new TablesGui(appRuntime);
            userDataQueriesGui = new UserDataQueriesGui(appRuntime.DataQueries);

            Control = new Panel
            {
                Width = 200,
                Padding = new Padding(5),
                
            };
            Control.Content = new TableLayout(
                tablesGui.Control,
                userDataQueriesGui.Control,
                new Panel());
        }

        public void Update(FrameTime frameTime)
        {
            tablesGui.Update(frameTime);
            userDataQueriesGui.Update(frameTime);
        }
    }
}