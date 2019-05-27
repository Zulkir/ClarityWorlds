using System;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.Objects.WorldTree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Helpers;
using Clarity.App.Worlds.Views;
using Valve.VR.InteractionSystem;
using Clarity.App.Worlds.Navigation;
using Assets.Scripts.Interaction.Tutorial;
using Assets.Scripts.Interaction;
using Assets.Scripts.Interaction.MoveInPlace;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Valve.VR;
using Assets.Scripts.Gui;

namespace Assets.Scripts.Infra
{
    public class TutorialScenarioOld
    {
        private readonly IWorldTreeService worldTreeService;
        private readonly INavigationService navigationService;
        private readonly IVrInputDispatcher vrInputDispatcher;
        private readonly IGlobalObjectService globalObjectService;
        private readonly IVrHeadPositionService headPositionService;
        private readonly IList<GameObject> cleanUpList;
        private  readonly int[] stageRoomIds;

        public TutorialScenarioOld(IWorldTreeService worldTreeService, INavigationService navigationService, IVrInputDispatcher vrInputDispatcher,
            IGlobalObjectService globalObjectService, IVrHeadPositionService headPositionService)
        {
            this.worldTreeService = worldTreeService;
            this.navigationService = navigationService;
            this.vrInputDispatcher = vrInputDispatcher;
            this.globalObjectService = globalObjectService;
            this.headPositionService = headPositionService;
            cleanUpList = new List<GameObject>();
            stageRoomIds = new int[6];
        }

        public void CleanUp()
        {
            foreach (var gameObject in cleanUpList)
                GameObject.Destroy(gameObject.gameObject);
        }

        public void Start()
        {
            var tutorialScene = worldTreeService.PresentationWorld.Scenes.FirstOrDefault();

            var index = 0;
            InitializeUiIntro(tutorialScene, index++);
            InitializeHeadPositioningIntro(tutorialScene, index++);
            InitializeFreeNavIntro(tutorialScene, index++);
            InitializeManipIntro(tutorialScene, index++);
            InitializeMinimapIntro(tutorialScene, index++);
            InitializeFinal(tutorialScene, index);
        }

        private void InitializeUiIntro(IScene tutorialScene, int stageIndex)
        {
            var uiIntroNode = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Buttons");
            stageRoomIds[stageIndex] = uiIntroNode.Id;

            var playerSpawn = uiIntroNode.SearchComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame;
            CreateRoomStartTrigger(playerSpawn.Eye.ToUnity(), VrInputDispatcherCapabilities.None, typeof(FixedTeleportVrNavigationMode));

            

            //Transition to next room
            var nextButtonPosition = playerSpawn.Eye.ToUnity() + new Vector3(0.4f, -1f, 0.3f);
            var nextButtonRotation = Quaternion.AngleAxis(45.0f, Vector3.up);
            var nextButtonText = "Move Next";
            var nextButtonObj = CreateButton(nextButtonPosition, nextButtonRotation, nextButtonText, () => 
            {
                SteamVR_Fade.Start(Color.white, 0);
                navigationService.GoToSpecific(stageRoomIds[stageIndex + 1]);
                SteamVR_Fade.Start(Color.clear, 0.5f);
                ControllerButtonHints.ShowTextHint(globalObjectService.VrLeftHand.GetComponentInChildren<Hand>(), SteamVR_Actions.default_RotateLeft, "rotate");
                ControllerButtonHints.ShowTextHint(globalObjectService.VrLeftHand.GetComponentInChildren<Hand>(), SteamVR_Actions.default_ResetPlayer, "reset head position");
            });
            cleanUpList.Add(nextButtonObj);
        }

        private void InitializeHeadPositioningIntro(IScene tutorialScene, int stageIndex)
        {
            var uiIntroNode = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Head Positioning");
            stageRoomIds[stageIndex] = uiIntroNode.Id;

            var playerSpawn = uiIntroNode.SearchComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame;
            CreateRoomStartTrigger(playerSpawn.Eye.ToUnity(), VrInputDispatcherCapabilities.None, typeof(FixedTeleportVrNavigationMode));

            //Transition to next room
            var nextButtonPosition = playerSpawn.Eye.ToUnity() + new Vector3(0.0f, -1f, -0.7f);
            var nextButtonRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
            var nextButtonText = "Press Me!";
            var nextButtonObj = CreateButton(nextButtonPosition, nextButtonRotation, nextButtonText, () =>
            {
                SteamVR_Fade.Start(Color.white, 0);
                navigationService.GoToSpecific(stageRoomIds[stageIndex + 1]);
                headPositionService.ResetHeadPosition();
                SteamVR_Fade.Start(Color.clear, 0.5f);

            });
            cleanUpList.Add(nextButtonObj);
        }

        private void InitializeFreeNavIntro(IScene tutorialScene, int stageIndex)
        {
            var nameTypePairs = new (string name, Type type)[]
            {
                ("Fixed Smooth Nav Intro", typeof(FixedSmoothVrNavigationMode)),
                ("Free Teleport Nav Intro", typeof(FreeTeleportVrNavigationMode)),
                ("Fixed Teleport Nav Intro", typeof(FixedTeleportVrNavigationMode)),
                ("Free Smooth Nav Intro", typeof(FreeSmoothVrNavigationMode)),
                ("Move-in-Place Nav Intro", typeof(MoveInPlaceVrNavigationMode)),
                ("Navigation Mode Switching", typeof(MoveInPlaceVrNavigationMode)),
                ("Free Navigation End", null),
            };

            var spawWithTypes = (from ntp in nameTypePairs
                let node = tutorialScene.EnumerateAllNodes().Single(x => x.Name == ntp.name)
                let spawn = node.GetComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame
                select (node: node, spawn: spawn, type: ntp.type)).ToArray();

            stageRoomIds[stageIndex] = spawWithTypes.First().node.Id;

            foreach (var (_, spawn, type) in spawWithTypes.ExceptLast().ExceptLast())
                CreateRoomStartTrigger(spawn.Eye.ToUnity(), VrInputDispatcherCapabilities.None, type);
            var modeSwitchingSpawnWithType = spawWithTypes[nameTypePairs.Length - 2];
            CreateRoomStartTrigger(modeSwitchingSpawnWithType.spawn.Eye.ToUnity(), VrInputDispatcherCapabilities.SwitchingModes, modeSwitchingSpawnWithType.type);

            var (_, lastSpawn, _) = spawWithTypes.Last();

            //Transition to next room
            var nextRoomTriggerPosition = lastSpawn.Eye.ToUnity();
            var nextRoomTriggerRotation = Quaternion.AngleAxis(90.0f, Vector3.up);

            var nextRoomTriggerObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Trigger"));
            nextRoomTriggerObj.transform.position = nextRoomTriggerPosition;
            nextRoomTriggerObj.transform.rotation = nextRoomTriggerRotation;
            nextRoomTriggerObj.transform.localScale *= 10;
            var nextRoomTriggerText = nextRoomTriggerObj.GetComponentInChildren<Text>();
            nextRoomTriggerText.text = "Come Here";

            var nextRoomTriggerHandler = nextRoomTriggerObj.AddComponent<FreeNavIntroCompleteHandler>();
            nextRoomTriggerHandler.GlobalObjectService = globalObjectService;
            nextRoomTriggerHandler.navigationService = navigationService;
            nextRoomTriggerHandler.GetNextRoomId = () => stageRoomIds[stageIndex + 1];

            cleanUpList.Add(nextRoomTriggerObj);
        }

        private void InitializeManipIntro(IScene tutorialScene, int stageIndex)
        {
            var manipIntroNode = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Manipulation");
            stageRoomIds[stageIndex] = manipIntroNode.Id;

            var playerSpawn = manipIntroNode.SearchComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame;
            CreateRoomStartTrigger(playerSpawn.Eye.ToUnity(), VrInputDispatcherCapabilities.SwitchingModes, typeof(FreeTeleportVrNavigationMode));

            //Transition to next room
            var nextRoomTriggerPosition = playerSpawn.Eye.ToUnity() + new Vector3(0.8f, -0.3f, 2.0f);
            var nextRoomTriggerObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Trigger"));
            nextRoomTriggerObj.transform.position = nextRoomTriggerPosition;
            var nextRoomTriggerText = nextRoomTriggerObj.GetComponentInChildren<Text>();
            nextRoomTriggerText.text = "Move the cube here";

            var nextRoomTriggerHandler = nextRoomTriggerObj.AddComponent<ManipIntroCompleteTriggerHandler>();
            //nextRoomTriggerHandler.navigationService = navigationService;
            //nextRoomTriggerHandler.GetNextRoomId = () => stageRoomIds[stageIndex + 1];

            cleanUpList.Add(nextRoomTriggerObj);
        }

        private void InitializeMinimapIntro(IScene tutorialScene, int stageIndex)
        {
            var minimapIntroNode = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Minimap");
            stageRoomIds[stageIndex] = minimapIntroNode.Id;

            var playerSpawn = minimapIntroNode.SearchComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame;
            CreateRoomStartTrigger(playerSpawn.Eye.ToUnity(), VrInputDispatcherCapabilities.All, typeof(FreeTeleportVrNavigationMode));
        }

        private void InitializeFinal(IScene tutorialScene, int stageIndex)
        {
            var node = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Final Room");
            stageRoomIds[stageIndex] = node.Id;

            var playerSpawn = node.SearchComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame;
            var buttonPos = playerSpawn.Eye.ToUnity() + new Vector3(0.0f, -1.1f, 1f);
            var buttonRot = Quaternion.identity;
            CreateButton(buttonPos, buttonRot, "Finish", () =>
            {
                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
            });
        }

        private static GameObject CreateButton(Vector3 position, Quaternion rotation, string text, Action onClick)
        {
            var canvasObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/VrGuiCanvas"));
            canvasObj.transform.SetPositionAndRotation(position,rotation);
            canvasObj.transform.localScale *= 2;

            var buttonObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/VrGuiButton"), canvasObj.transform);
            buttonObj.GetComponentInChildren<Text>().text = text;

            buttonObj.GetComponent<UIElement>().onHandClick.AddListener(h => onClick());

            return buttonObj;
        }

        private GameObject CreateRoomStartTrigger(Vector3 position, VrInputDispatcherCapabilities capabilities, Type vrNavigationModeType)
        {
            var roomStartTriggerObj = new GameObject();
            roomStartTriggerObj.transform.position = position;
            //roomStartTriggerObj.transform.localScale *= 10;

            var roomStartTriggerHandler = roomStartTriggerObj.AddComponent<TutorialRoomStartTrigger>();
            roomStartTriggerHandler.GlobalObjectService = globalObjectService;
            roomStartTriggerHandler.VrInputDispatcher = vrInputDispatcher;
            roomStartTriggerHandler.Capabilities = capabilities;
            roomStartTriggerHandler.VrNavigationModeType = vrNavigationModeType;

            return roomStartTriggerObj;
        }
    }
}
