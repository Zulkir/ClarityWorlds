using System;
using Clarity.Core.AppCore.AppModes;
using Clarity.Ext.Gui.EtoForms.Helpers;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.AppModes
{
    public interface IAppModesCommands
    {
        Command StartPresentation { get; }
        Command StopPresentation { get; }
    }

    public class AppModesCommands : IAppModesCommands
    {
        private readonly IAppModeService appModeService;
        private readonly Lazy<GuiEto> guiLazy;
        public Command StartPresentation { get; }
        public Command StopPresentation { get; }

        private GuiEto Gui => guiLazy.Value;

        public AppModesCommands(IAppModeService appModeService, Lazy<GuiEto> guiLazy)
        {
            this.appModeService = appModeService;
            this.guiLazy = guiLazy;
            StartPresentation = GuiCommandsHelper.Create("Start Presentation", ExecStartPresentation, Keys.F5);
            StopPresentation = GuiCommandsHelper.Create("Stop Presentation", ExecStopPresentation, Keys.Escape);
            appModeService.ModeChanged += OnModeChanged;
            OnModeChanged();
        }

        private void OnModeChanged()
        {
            StartPresentation.Enabled = appModeService.Mode != AppMode.Presentation;
            StopPresentation.Enabled = appModeService.Mode == AppMode.Presentation;
        }

        private void ExecStartPresentation(object sender, EventArgs eventArgs)
        {
            appModeService.SetMode(AppMode.Presentation);
        }

        private void ExecStopPresentation(object sender, EventArgs eventArgs)
        {
            appModeService.SetMode(AppMode.Editing);
        }
    }
}