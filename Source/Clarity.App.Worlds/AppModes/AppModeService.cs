﻿using Clarity.Engine.EventRouting;

namespace Clarity.App.Worlds.AppModes
{
    public class AppModeService : IAppModeService
    {
        public AppMode Mode { get; private set; }
        public AppNavigationMode NavigationMode { get; private set; }

        private readonly IEventRoutingService eventRoutingService;

        public AppModeService(IEventRoutingService eventRoutingService)
        {
            this.eventRoutingService = eventRoutingService;
        }

        public void SetMode(AppMode mode)
        {
            Mode = mode;
            eventRoutingService.FireEvent<IAppModeChangedEvent>(new AppModeChangedEvent(mode));
        }

        public void SetNavigationMode(AppNavigationMode navigationMode)
        {
            NavigationMode = navigationMode;
            // todo Fire Event
        }
    }
}