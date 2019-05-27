using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Gui;
using Assets.Scripts.Infra;
using Assets.Scripts.Interaction.Minimap;
using Clarity.App.Worlds.Interaction.Queries;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui.MessagePopups;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using UnityEngine;
using Valve.VR;

namespace Assets.Scripts.Interaction
{
    public class AdvancedVrInputDispatcher : IVrInputDispatcher
    {
        private readonly IMessagePopupService messagePopupService;
        private readonly IMinimapVrNavigationMode minimapVrNavigationMode;
        private readonly IUserQueryService userQueryService;
        private readonly IVrHeadPositionService headPositionService;
        private readonly IGlobalObjectService globalObjectService;
        private VrInputDispatcherCapabilities capabilities;
        private int? currentModeIndex;
        private bool initialized;
        private bool queryWasShown;

        public IReadOnlyList<IVrNavigationMode> NavigationModes { get; }
        private IVrNavigationMode CurrentNavigationMode => currentModeIndex.HasValue ? NavigationModes[currentModeIndex.Value] : null;

        private const float HintsSeconds = 3f;

        public AdvancedVrInputDispatcher(IMessagePopupService messagePopupService, IReadOnlyList<IVrNavigationMode> navigationModes, 
            IEventRoutingService eventRoutingService, IUserQueryService userQueryService,
            IVrHeadPositionService headPositionService, IGlobalObjectService globalObjectService)
        {
            this.messagePopupService = messagePopupService;
            this.userQueryService = userQueryService;
            this.headPositionService = headPositionService;
            this.globalObjectService = globalObjectService;
            this.NavigationModes = navigationModes.Where(x => !(x is IMinimapVrNavigationMode)).ToArray();
            this.minimapVrNavigationMode = navigationModes.OfType<IMinimapVrNavigationMode>().Single();
            minimapVrNavigationMode.ZoomEnabled = true;
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(IVrInputDispatcher), nameof(OnNewFrame), OnNewFrame);
            eventRoutingService.Subscribe<IFixedUpdateEvent>(typeof(IVrInputDispatcher), nameof(OnFixedUpdate), OnFixedUpdate);
            capabilities = VrInputDispatcherCapabilities.All;
        }

        public void Initialize()
        {
            minimapVrNavigationMode.Initialize();
            foreach (var mode in NavigationModes)
                mode.Initialize();
            currentModeIndex = 0;
            NavigationModes[currentModeIndex.Value].SetEnabled(true);
            initialized = true;
        }

        public void SetCapabilities(VrInputDispatcherCapabilities capabilities)
        {
            this.capabilities = capabilities;
        }

        public void NavigateTo(ISceneNode node, IStoryPath storyPath, CameraProps initialCameraProps, float? initialFloorHeight, float? targetFloorHeight)
        {
            FixedTeleportVrNavigationMode.TeleportTo(globalObjectService.VrPlayerCarrier, node, targetFloorHeight);
        }

        public bool MinimapIsOpen
        {
            get => minimapVrNavigationMode.IsEnabled;
            set
            {
                var showMinimap = value;
                CurrentNavigationMode?.SetEnabled(!showMinimap);
                minimapVrNavigationMode.SetEnabled(showMinimap);
            }
        }

        private void OnNewFrame(INewFrameEvent evnt)
        {
            if (!initialized)
                return;

            if (!queryWasShown && userQueryService.Queries.HasItems())
            {
                minimapVrNavigationMode.SetEnabled(false);
                CurrentNavigationMode?.SetEnabled(false);
                queryWasShown = true;
            }
            else if (queryWasShown && !userQueryService.Queries.HasItems())
            {
                CurrentNavigationMode?.SetEnabled(true);
                queryWasShown = false;
            }

            if (!queryWasShown)
            {
                if (capabilities.HasFlag(VrInputDispatcherCapabilities.Minimap) &&
                    SteamVR_Actions.default_MinimapToggle.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    MinimapIsOpen = !MinimapIsOpen;

                if (capabilities.HasFlag(VrInputDispatcherCapabilities.SwitchingModes) && 
                    !MinimapIsOpen && 
                    SteamVR_Actions.default_ChangeMode.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    CurrentNavigationMode?.HideHints();
                    CurrentNavigationMode?.SetEnabled(false);
                    currentModeIndex = (currentModeIndex + 1) % NavigationModes.Count;
                    CurrentNavigationMode.SetEnabled(true);
                    CurrentNavigationMode.ShowHints(HintsSeconds);
                    var message = $"Navigation: {CurrentNavigationMode.UserFriendlyName}";
                    Debug.Log(message);
                    messagePopupService.Show(message);
                }
            }

            if (!MinimapIsOpen)
            {
                if (SteamVR_Actions.default_RotateLeft.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    headPositionService.RotateRight(-45);
                if (SteamVR_Actions.default_RotateRight.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    headPositionService.RotateRight(45);
                if (SteamVR_Actions.default_ElevatorUp.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    headPositionService.TryUseElevator(true);
                else if (SteamVR_Actions.default_ElevatorDown.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    headPositionService.TryUseElevator(false);
            }

            if (SteamVR_Actions.default_ResetPlayer.GetStateDown(SteamVR_Input_Sources.Any))
                headPositionService.ResetHeadPosition();

            minimapVrNavigationMode.Update(evnt.FrameTime);
            foreach (var mode in NavigationModes)
                mode.Update(evnt.FrameTime);
        }

        private void OnFixedUpdate(IFixedUpdateEvent evnt)
        {
            foreach (var mode in NavigationModes)
                mode.FixedUpdate();
        }

        public void SetVrNavigationMode(Type type, bool showHints)
        {
            var index = type != null ? NavigationModes.IndexOf(type.IsInstanceOfType) : null;
            SetVrNavigationModeIndex(index, showHints);
        }

        public void SetVrNavigationModeIndex(int? id, bool showHints)
        {
            CurrentNavigationMode?.SetEnabled(false);
            currentModeIndex = id;
            CurrentNavigationMode?.SetEnabled(true);
            if (showHints)
                CurrentNavigationMode?.ShowHints(HintsSeconds);
        }
    }
}