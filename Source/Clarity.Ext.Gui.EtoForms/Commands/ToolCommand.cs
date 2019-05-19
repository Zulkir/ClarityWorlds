using System;
using Clarity.Core.AppCore.Tools;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Commands
{
    public class ToolCommand : Command
    {
        private readonly IToolService toolService;
        private readonly Func<ITool> createTool;

        public ToolCommand(string name, IToolService toolService, Func<ITool> createTool)
        {
            this.createTool = createTool;
            this.toolService = toolService;

            MenuText = name;
            ToolBarText = name;
        }

        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            if (!Enabled)
                return;
            var tool = createTool();
            if (tool != null)
                toolService.CurrentTool = tool;
        }
    }
}