using Assets.Scripts.Gui;
using Assets.Scripts.Infra;
using Assets.Scripts.Interaction.Minimap;
using Assets.Scripts.Interaction.MoveInPlace;
using Clarity.App.Worlds.Interaction.Queries;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui.MessagePopups;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using Valve.VR;

namespace Assets.Scripts.Interaction
{
    class NoviceVrInputDispatcher : IVrInputDispatcher
    {
        private readonly IMinimapVrNavigationMode minimapVrNavigationMode;
        private readonly IVrHeadPositionService headPositionService;
        private readonly IGlobalObjectService globalObjectService;
        private readonly IVrManipulationService vrManipulationService;
        private VrInputDispatcherCapabilities capabilities;
        private bool initialized;
        private bool navigationEnabled;

        private readonly FreeTeleportVrNavigationMode freeTeleportMode;
        private readonly MoveInPlaceVrNavigationMode moveInPlaceMode;

        public IReadOnlyList<IVrNavigationMode> NavigationModes { get; }
        private IVrNavigationMode CurrentNavigationMode => null;

        private const float HintsSeconds = 3f;

        public NoviceVrInputDispatcher(IMessagePopupService messagePopupService, IReadOnlyList<IVrNavigationMode> navigationModes,
            IEventRoutingService eventRoutingService, IUserQueryService userQueryService,
            IVrHeadPositionService headPositionService, IGlobalObjectService globalObjectService, IVrManipulationService vrManipulationService)
        {
            this.headPositionService = headPositionService;
            this.globalObjectService = globalObjectService;
            this.vrManipulationService = vrManipulationService;
            minimapVrNavigationMode = navigationModes.OfType<IMinimapVrNavigationMode>().Single();
            minimapVrNavigationMode.ZoomEnabled = false;
            freeTeleportMode = navigationModes.OfType<FreeTeleportVrNavigationMode>().Single();
            moveInPlaceMode = navigationModes.OfType<MoveInPlaceVrNavigationMode>().Single();
            NavigationModes = new IVrNavigationMode[] { freeTeleportMode, moveInPlaceMode };
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(IVrInputDispatcher), nameof(OnNewFrame), OnNewFrame);
            eventRoutingService.Subscribe<IFixedUpdateEvent>(typeof(IVrInputDispatcher), nameof(OnFixedUpdate), OnFixedUpdate);
            capabilities = VrInputDispatcherCapabilities.All;
        }

        public void Initialize()
        {
            minimapVrNavigationMode.Initialize();
            freeTeleportMode.Initialize();
            moveInPlaceMode.Initialize();
            freeTeleportMode.SetEnabled(true);
            moveInPlaceMode.SetEnabled(true);
            navigationEnabled = true;
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
                minimapVrNavigationMode.SetEnabled(showMinimap);
                SetNavigationEnabled(!value);
            }
        }

        private void OnNewFrame(INewFrameEvent evnt)
        {
            if (!initialized)
                return;

            if (capabilities.HasFlag(VrInputDispatcherCapabilities.Minimap) &&
                    SteamVR_Actions.default_MinimapToggle.GetStateDown(SteamVR_Input_Sources.LeftHand))
                MinimapIsOpen = !MinimapIsOpen;

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


            if (!vrManipulationService.GetGrabbedObjects().Any() && !moveInPlaceMode.IsEnabled)
            {
                moveInPlaceMode.SetEnabled(true);
            }
            else if(vrManipulationService.GetGrabbedObjects().Any() && moveInPlaceMode.IsEnabled)
            {
                moveInPlaceMode.SetEnabled(false);
            }

            minimapVrNavigationMode.Update(evnt.FrameTime);
            foreach (var mode in NavigationModes)
                mode.Update(evnt.FrameTime);




            foreach (var mode in NavigationModes)
                mode.Update(evnt.FrameTime);
        }

        private void OnFixedUpdate(IFixedUpdateEvent evnt)
        {
            foreach (var mode in NavigationModes)
                mode.FixedUpdate();
        }

        public void SetNavigationEnabled(bool enable)
        {
            if (enable && !navigationEnabled)
            {
                foreach (var mode in NavigationModes)
                    mode.SetEnabled(true);
                navigationEnabled = true;
            }
            else if (!enable && navigationEnabled)
            {
                foreach (var mode in NavigationModes)
                    mode.SetEnabled(false);
                navigationEnabled = false;
            }
        }

        public void SetVrNavigationMode(Type type, bool showHints)
        {
            SetNavigationEnabled(type != null);
        }

        public void SetVrNavigationModeIndex(int? id, bool showHints)
        {
            SetNavigationEnabled(id.HasValue);
        }
    }
}
