using System;
using Clarity.App.Worlds.AppModes;
using Clarity.Engine.EventRouting;
using Clarity.Ext.Gui.EtoForms.Helpers;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.AppModes
{
    public class AppModesCommands : IAppModesCommands
    {
        private readonly IAppModeService appModeService;
        public Command StartPresentation { get; }
        public Command StopPresentation { get; }

        public AppModesCommands(IEventRoutingService eventRoutingService, IAppModeService appModeService)
        {
            this.appModeService = appModeService;
            StartPresentation = GuiCommandsHelper.Create("Start Presentation", ExecStartPresentation, Keys.F5);
            StopPresentation = GuiCommandsHelper.Create("Stop Presentation", ExecStopPresentation, Keys.Escape);
            eventRoutingService.Subscribe<IAppModeChangedEvent>(typeof(IAppModesCommands), nameof(OnModeChanged), OnModeChanged);
            OnModeChanged(new AppModeChangedEvent(appModeService.Mode));
        }

        private void OnModeChanged(IAppModeChangedEvent appModeChangedEvent)
        {
            StartPresentation.Enabled = appModeChangedEvent.NewAppMode != AppMode.Presentation;
            StopPresentation.Enabled = appModeChangedEvent.NewAppMode == AppMode.Presentation;
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