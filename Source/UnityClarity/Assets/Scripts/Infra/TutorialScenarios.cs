using System;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Interaction;
using Assets.Scripts.Interaction.Tutorial;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.Infra
{
    public class TutorialScenario
    {
        private readonly IWorldTreeService worldTreeService;
        private readonly IVrInputDispatcher inputDispatcher;
        private readonly INavigationService navigationService;
        private readonly ICoroutineService coroutineService;
        private readonly IGlobalObjectService globalObjectService;
        private readonly IVrManipulationService manipulationService;

        public TutorialScenario(IWorldTreeService worldTreeService, IVrInputDispatcher inputDispatcher,
            INavigationService navigationService, ICoroutineService coroutineService, IGlobalObjectService globalObjectService, 
            IVrManipulationService manipulationService)
        {
            this.worldTreeService = worldTreeService;
            this.inputDispatcher = inputDispatcher;
            this.navigationService = navigationService;
            this.coroutineService = coroutineService;
            this.globalObjectService = globalObjectService;
            this.manipulationService = manipulationService;
        }

        private static GameObject CreateButton(Vector3 position, Quaternion rotation, string text, Action onClick)
        {
            var canvasObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/VrGuiCanvas"));
            canvasObj.transform.SetPositionAndRotation(position, rotation);
            canvasObj.transform.localScale *= 2;

            var buttonObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/VrGuiButton"), canvasObj.transform);
            buttonObj.GetComponentInChildren<Text>().text = text;

            buttonObj.GetComponent<UIElement>().onHandClick.AddListener(h => onClick());

            return buttonObj;
        }

        public async void RunTutorial()
        {
            var tutorialScene = worldTreeService.PresentationWorld.Scenes.FirstOrDefault();

            var leftHand = globalObjectService.VrLeftHand.GetComponentInChildren<Hand>();
            var rightHand = globalObjectService.VrRightHand.GetComponentInChildren<Hand>();

            {
                var uiIntroNode = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Buttons");
                navigationService.GoToSpecific(uiIntroNode.Id);

                var playerSpawn = uiIntroNode.SearchComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame;
                inputDispatcher.SetCapabilities(VrInputDispatcherCapabilities.None);
                inputDispatcher.SetVrNavigationMode(null);

                var nextButtonPosition = playerSpawn.Eye.ToUnity() + new Vector3(0.15f, -1f, 0.05f);
                var nextButtonRotation = Quaternion.AngleAxis(45.0f, Vector3.up);
                var nextButtonText = "Move Next";

                var buttonPressed = false;
                var nextButtonObj = CreateButton(nextButtonPosition, nextButtonRotation, nextButtonText, () => buttonPressed = true);

                ControllerButtonHints.ShowTextHint(rightHand, SteamVR_Actions.default_ResetPlayer, "Press trigger to interact with UI");
                await coroutineService.WaitCondition(() => buttonPressed);
                ControllerButtonHints.HideAllTextHints(rightHand);

                GameObject.Destroy(nextButtonObj);
            }
            {
                var node = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Head Positioning");
                SteamVR_Fade.Start(Color.white, 0);
                navigationService.GoToSpecific(node.Id);
                SteamVR_Fade.Start(Color.clear, 0.5f);

                ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_ResetPlayer, "Press X to reset head position");
                await coroutineService.WaitCondition(() => SteamVR_Actions.default_ResetPlayer.state);
                ControllerButtonHints.HideAllTextHints(leftHand);

                ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_RotateLeft, "Move joystick to rotate camera");
                await coroutineService.WaitCondition(() =>
                    SteamVR_Actions.default_RotateLeft.state || SteamVR_Actions.default_RotateRight.state);
                ControllerButtonHints.HideAllTextHints(leftHand);

                //Transition to next room
                var playerPos = globalObjectService.VrPlayerCarrier.transform.position;
                var nextButtonPosition = playerPos + new Vector3(0.7f, 1f, 0f);
                var nextButtonRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
                var nextButtonText = "Move Next";

                var buttonPressed = false;
                var nextButtonObj = CreateButton(nextButtonPosition, nextButtonRotation, nextButtonText, () => buttonPressed = true);
                await coroutineService.WaitCondition(() => buttonPressed);

                GameObject.Destroy(nextButtonObj);
            }
            {
                inputDispatcher.SetVrNavigationModeIndex(0);

                var node = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Move-in-Place Nav Intro");
                SteamVR_Fade.Start(Color.white, 0);
                navigationService.GoToSpecific(node.Id);
                SteamVR_Fade.Start(Color.clear, 0.5f);

                var nextNode = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Free Teleport Nav Intro");
                var nextNodeBounds = new AaBox(nextNode.GlobalTransform.Offset, new Size3(3, 2, 4));

                ControllerButtonHints.ShowTextHint(rightHand, SteamVR_Actions.default_GrabGrip, "Hold and move up and down");
                ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_GrabGrip, "Hold and move up and down");
                await coroutineService.WaitCondition(() => nextNodeBounds.Contains(globalObjectService.VrPlayer.transform.position.ToClarity()));
                await coroutineService.WaitSeconds(0.5f);
                ControllerButtonHints.HideAllTextHints(rightHand);
                ControllerButtonHints.HideAllTextHints(leftHand);

                nextNode = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Free Navigation End");
                nextNodeBounds = new AaBox(nextNode.GlobalTransform.Offset, new Size3(3, 2, 4));

                ControllerButtonHints.ShowTextHint(rightHand, SteamVR_Actions.default_Teleport, "Press and choose new location");
                await coroutineService.WaitCondition(() => nextNodeBounds.Contains(globalObjectService.VrPlayer.transform.position.ToClarity()));
                await coroutineService.WaitUpdates(2);
                ControllerButtonHints.HideAllTextHints(rightHand);
            }
            {
                var node = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Manipulation");
                SteamVR_Fade.Start(Color.white, 0);
                navigationService.GoToSpecific(node.Id);
                SteamVR_Fade.Start(Color.clear, 0.5f);
                var playerSpawn = node.SearchComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame;

                //Transition to next room
                var nextRoomTriggerPosition = playerSpawn.Eye.ToUnity() + new Vector3(0.4f, -0.3f, 2.5f);
                var nextRoomTriggerObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Trigger"));
                nextRoomTriggerObj.transform.position = nextRoomTriggerPosition;
                var nextRoomTriggerText = nextRoomTriggerObj.GetComponentInChildren<Text>();
                nextRoomTriggerText.text = "Move the cube here";

                var nextRoomTriggerHandler = nextRoomTriggerObj.AddComponent<ManipIntroCompleteTriggerHandler>();

                ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_GrabGrip, "Grab the cube");
                await coroutineService.WaitCondition(() => manipulationService.GetGrabbedObjects().Any(x => x.Name == "ManipIntroCube"));
                ControllerButtonHints.HideAllTextHints(leftHand);

                ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_GrabGrip, "Move the cube to the designated place");
                await coroutineService.WaitCondition(() => nextRoomTriggerHandler.Triggered);
                await coroutineService.WaitUpdates(2);
                ControllerButtonHints.HideAllTextHints(leftHand);

                GameObject.Destroy(nextRoomTriggerHandler);
            }
            {
                var node = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Minimap");
                SteamVR_Fade.Start(Color.white, 0);
                navigationService.GoToSpecific(node.Id);
                SteamVR_Fade.Start(Color.clear, 0.5f);
                inputDispatcher.SetCapabilities(VrInputDispatcherCapabilities.All);

                ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_MinimapToggle, "Press to open the minimap");
                await coroutineService.WaitCondition(() => inputDispatcher.MinimapIsOpen);
                ControllerButtonHints.HideAllTextHints(leftHand);

                var nextNode = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Final Room");
                var nextNodeBounds = new AaBox(nextNode.GlobalTransform.Offset, new Size3(3, 2, 4));

                ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_MinimapToggle, "Toggle the minimap");
                ControllerButtonHints.ShowTextHint(rightHand, SteamVR_Actions.default_InteractUI, "Point to the topmost room and press");
                await coroutineService.WaitCondition(() => nextNodeBounds.Contains(globalObjectService.VrPlayer.transform.position.ToClarity()));
                ControllerButtonHints.HideAllTextHints(rightHand);
                ControllerButtonHints.HideAllTextHints(leftHand);
                inputDispatcher.MinimapIsOpen = false;
            }
            {
                var node = tutorialScene.EnumerateAllNodes().Single(x => x.Name == "Final Room");
                var playerSpawn = node.SearchComponent<IFocusNodeComponent>().DefaultViewpointMechanism.FixedCamera.GetProps().Frame;
                var buttonPos = playerSpawn.Eye.ToUnity() + new Vector3(0.0f, -1.1f, 1f);
                var buttonRot = Quaternion.identity;
                CreateButton(buttonPos, buttonRot, "Finish", () =>
                {
                    Application.Quit();
                    UnityEditor.EditorApplication.isPlaying = false;
                });
            }
        }
    }
}